namespace HSRecord.Core.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Linq;
    using HSRecord.Core.Entities;
    using Newtonsoft.Json;
    public class HSParser2
    {
        private string fileName;
        private string[] lines;
        private string content;
        private string[] ignoreTag =
        {
            "STEP", 
            "NEXT_STEP",
            //"TURN",
            "NUM_TURNS_LEFT",
            "NUM_TURNS_IN_PLAY",
            "NUM_CARDS_DRAWN_THIS_TURN",
            "NUM_MINIONS_PLAYED_THIS_TURN",
            "NUM_TIMES_HERO_POWER_USED_THIS_GAME",
            "NUM_OPTIONS_PLAYED_THIS_TURN",
            "LAST_CARD_PLAYED",
            "COMBO_ACTIVE",
            "JUST_PLAYED",
            "RESOURCES_USED",
            "EXHAUSTED",
            "HEROPOWER_ACTIVATIONS_THIS_TURN",
            "COST",
            "PROPOSED_ATTACKER",
            "PROPOSED_DEFENDER",
            "ATTACKING",
            "DEFENDING",
            "NUM_MINIONS_PLAYER_KILLED_THIS_TURN",
            "NUM_MINIONS_KILLED_THIS_TURN",
            "NUM_MINIONS_KILLED_THIS_TURN",
            "IGNORE_DAMAGE",
            "IGNORE_DAMAGE_OFF"
        };

        public Game ParseGame(string fileName)
        {
            var game = new Game();
            this.Parse(fileName);
            var actions = this.ParseActions();
            foreach (GameAction action in actions)
            {
                HandleAction(game, action);
            }
            return game;
        }

        public void HandleAction(Game game, GameAction action)
        {
            foreach (Tag tag in action.Tags)
            {
                HandleTag(game, action, tag);
            }
        }

        public void HandleTag(Game game, GameAction action, Tag tag)
        {
            if (string.Equals(tag.Name, "TURN"))
                HandleTurnTag(game, action, tag);

            if (string.Equals(tag.Name, "ZONE") &&
                string.Equals(tag.Value, "HAND") &&
                string.Equals(action.SubType, "TRIGGER"))
                HandleTurnDrawTag(game, action, tag);
        }

        public void HandleTurnTag(Game game, GameAction action, Tag tag)
        {
            var turn = new Turn();
            turn.TurnIndex = int.Parse(tag.Value);
            if (game.Draw != null)
            {
                turn.Draw = game.Draw;
                game.Draw = null;
            }

            game.Turns.Add(turn);
        }
        public void HandleTurnDrawTag(Game game, GameAction action, Tag tag)
        {
            game.Draw = tag.Entity.Card;
        }

        public void Parse(string fileName)
        {
            this.fileName = fileName;
            this.Read();
            // Console.WriteLine("Total lines :{0}", this.lines.Length);
            for (var i = 0; i < this.lines.Length; i++)
                this.ParseLine(lines[i]);
        }

        private void Read()
        {
            var builder = new StringBuilder();
            this.lines = File.ReadAllLines(fileName);
            //this.content = File.ReadAllText(fileName);
            for (var i = 0; i < this.lines.Length; i++)
                this.lines[i].Replace("ZoneChangeList.ProcessChanges()", string.Empty);
        }

        public List<string> ParseActionsString()
        {
            var actionRegex = new Regex("ACTION_START [^>]*ACTION_END");
            var actionMatches = actionRegex.Matches(this.content);
            var actionStrings = new List<string>();
            foreach (Match match in actionMatches)
                actionStrings.Add(match.Value);

            return actionStrings;
        }

        public List<GameAction> ParseActions()
        {
            var actionStrings = this.ParseActionsString();
            var actions = new List<GameAction>();

            for (var i = 0; i < actionStrings.Count; i++)
                actions.Add(ParseAction(actionStrings[i]));

            return actions;
        }

        public string ParseActionsJson()
        {
            return JsonConvert.SerializeObject(ParseActions());
        }

        private GameAction ParseAction(string actionString)
        {
            var action = ParseActionHeader(actionString);
            action.Tags = ParseTags(actionString);

            return action;
        }

        private GameAction ParseActionHeader(string actionString)
        {
            var action = new GameAction();
            var headerRegex =
                new Regex(@"ACTION_START\sEntity=(\[name=(?<entityname>[\w\s\dа-яА-Я_-]+)\sid=(?<entityid>\d+)\szone=(?<entityzone>\w+)\szonePos=(?<entityzonepos>\d)\scardId=(?<entitycardid>[\w\d_]+)\splayer=(?<entityplayer>\d)\]|(?<entityglobalname>[\w\s\d-_]*))\sSubType=(?<subtype>\w+)\sIndex=(?<index>[-\d]+)\sTarget=(\[name=(?<targetname>[\w\s\dа-яА-Я_-]+)\sid=(?<targetid>\d+)\szone=(?<targetzone>\w+)\szonePos=(?<targetzonepos>\d)\scardId=(?<targetdardid>[\w\d_]+)\splayer=(?<targetplayer>\d)\]|(?<targetglobalname>[\w\s\d-_]*))");
            var match = headerRegex.Match(actionString);
            action.SubType = match.Groups["subtype"].Value;
            action.IsConsistent = true;
            int index = 0;
            if (int.TryParse(match.Groups["index"].Value, out index))
                action.Index = index;

            if (match.Groups["entityglobalname"].Value == string.Empty)
            {
                try
                {
                    action.Entity.Name = match.Groups["entityname"].Value;
                    action.Entity.Id = int.Parse(match.Groups["entityid"].Value);
                    action.Entity.CardId = match.Groups["entitycardid"].Value;
                    action.Entity.ZonePos = int.Parse(match.Groups["entityzonepos"].Value);
                    action.Entity.Zone = match.Groups["entityzone"].Value;
                    action.Entity.Player = int.Parse(match.Groups["entityplayer"].Value);
                }
                catch
                {
                    action.Entity.Type = "INVALID";
                    action.IsConsistent = false;
                }
            }
            else
            {
                action.Entity.Name = match.Groups["entityglobalname"].Value;
            }
            if (match.Groups["targetglobalname"].Value == string.Empty)
            {
                try
                {
                    action.Target.Name = match.Groups["targetname"].Value;
                    action.Target.Id = int.Parse(match.Groups["targetid"].Value);
                    action.Target.CardId = match.Groups["targetcardid"].Value;
                    action.Target.ZonePos = int.Parse(match.Groups["targetzonepos"].Value);
                    action.Target.Zone = match.Groups["targetzone"].Value;
                    action.Target.Player = int.Parse(match.Groups["targetplayer"].Value);
                }
                catch
                {
                    action.Target.Type = "INVALID";
                    action.IsConsistent = false;
                }
            }
            else
            {
                action.Target.Name = match.Groups["targetglobalname"].Value;
            }
            return action;
        }
        private List<Tag> ParseTags(string actionString)
        {
            var tagRegex = new Regex("TAG_CHANGE (.*)");
            var tagMatches = tagRegex.Matches(actionString);
            var tags = new List<Tag>();
            foreach (Match tagMatch in tagMatches)
            {
                var tag = this.ParseTag(tagMatch.Value);
                if (tag.IsConsistent && !ignoreTag.Contains(tag.Name))
                    tags.Add(tag);
            }
            return tags;
        }
        private Tag ParseTag(string tagString)
        {
            var tag = new Tag();
            var tagRegex = new Regex(@"Entity=(\[name=(?<entityname>[\w\s\dа-яА-Я_-]+)\sid=(?<entityid>\d+)\szone=(?<entityzone>\w+)\szonePos=(?<entityzonepos>\d)\scardId=(?<entitycardid>[\w\d_]+)\splayer=(?<entityplayer>\d)\]|\[id=(?<entityid>\d+)\scardId=\stype=INVALID\szone=(?<entityzone>\w*)\szonePos=(?<entityzonepos>\d+)\splayer=(?<entityplayer>\d)\]|(?<entity>.*)) tag=(?<tag>[\w_]*) value=(?<value>[^\r]*)");
            var match = tagRegex.Match(tagString);
            tag.Entity = new Entity();
            tag.IsConsistent = true;
            int entityId;
            if (!string.IsNullOrEmpty(match.Groups["entityid"].Value) &&
                int.TryParse(match.Groups["entityid"].Value, out entityId))
            {
                tag.Entity.Id = entityId;
                if (string.IsNullOrEmpty(match.Groups["entityname"].Value))
                {
                    tag.Entity.Id = entityId;
                    tag.Entity.Zone = match.Groups["entityzone"].Value;
                    tag.Entity.ZonePos = int.Parse(match.Groups["entityzonepos"].Value);
                    tag.Entity.Player = int.Parse(match.Groups["entityplayer"].Value);
                }
                else
                {
                    tag.Entity.Name = match.Groups["entityname"].Value;
                    tag.Entity.CardId = match.Groups["entitycardid"].Value;
                    tag.Entity.Zone = match.Groups["entityzone"].Value;
                    tag.Entity.ZonePos = int.Parse(match.Groups["entityzonepos"].Value);
                    tag.Entity.Player = int.Parse(match.Groups["entityplayer"].Value);
                }
            }
            else
            {
                tag.Entity.Name = match.Groups["entity"].Value;
            }
            tag.Name = match.Groups["tag"].Value;
            tag.Value = match.Groups["value"].Value;
            return tag;
        }

        private List<int> TurnLineIndexes()
        {
            var indexes = new List<int>();
            for (var i = 0; i < lines.Length; i++)
                if (lines[i].Contains("TAG_CHANGE Entity=GameEntity tag=STEP value=MAIN_READY"))
                    indexes.Add(i);

            return indexes;
        }

        private int GameFinalIndex()
        {
            for (var i = 0; i < lines.Length; i++)
                if (lines[i].Contains("TAG_CHANGE Entity=GameEntity tag=STATE value=COMPLETE"))
                    return i;

            return lines.Length;
        }

        private List<Turn> ParseTurns()
        {
            var turns = new List<Turn>();
            var turnIndexes = TurnLineIndexes();
            turnIndexes.Add(GameFinalIndex());
            for (var i = 1; i <= turnIndexes.Count; i++)
                ParseTurn(i - 1, i);

            return turns;
        }

        private Turn ParseTurn(int startIndex, int endIndex)
        {
            //for(var i = startIndex;i<=endIndex;i++)
            throw new NotFiniteNumberException();

        }

        private void ParseLine(string line)
        {
            if (line.Contains("SubType=ATTACK"))
                this.Attack(line);
        }

        private void Attack(string line)
        {
            var entityRegex = new Regex(@"Entity=\[name=([\w\s\d_-]+)\sid=(\d+)\szone=(\w+)\szonePos=(\d)\scardId=([\w\d_]+)\splayer=(\d)\]");
            var targetRegex = new Regex(@"Target=\[name=([\w\s\d_-]+)\sid=(\d+)\szone=(\w+)\szonePos=(\d)\scardId=([\w\d_]+)\splayer=(\d)");

            var entityMatch = entityRegex.Match(line);
            var targetMatch = targetRegex.Match(line);

            // Console.WriteLine("Attacker: {0}, Defender: {1}", entityMatch.Groups[1].Captures[0].Value, targetMatch.Groups[1].Captures[0].Value);
        }
    }
}
