using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using HSRecord.Core.Acts;

namespace HSRecord.Core.Parsing
{
    using System.IO;
    using HSRecord.Core.Entities;
    public class HSParser
    {
        private string fileName;
        private string[] lines;
        private Game game;
        private Turn CurrentTurn;
        private string CurrentPlayer;
        private int CurrentTurnIndex;
        public Game ParseGame(string fileName)
        {
            this.fileName = fileName;
            this.game = new Game();
            this.Parse();
            return game;
        }
        public void Parse()
        {
            this.Read();

            for (var i = 0; i < this.lines.Length; i++)
            {
                var line = this.lines[i];
                if (line.Contains("TRANSITIONING"))
                    i = HandleTransitioning(i);
                else if (line.Contains("ACTION_START"))
                    i = HandleActionStart(i);
                else if (line.Contains("TAG_CHANGE"))
                    i = HandleTagChange(i);
            }
        }

        private int HandleTransitioning(int lineIndex)
        {
            if (this.CurrentTurn != null)
            {
                var entity = ParseDraw(this.lines[lineIndex]);
                this.CurrentTurn.Acts.Add(new DrawCardAct(entity.Card));
            }
            return lineIndex;
        }

        private int HandleTagChange(int lineIndex)
        {
            var tag = ParseTag(this.lines[lineIndex]);
            TryHandleStepTag(tag);
            TryHandleCurrentPlayerTag(tag);
            TryHandleTurnTag(tag);
            return lineIndex;
        }

        private void TryHandleStepTag(Tag tag)
        {
            if (string.Equals(tag.Name, "STEP") &&
                string.Equals(tag.Value, "MAIN_READY"))
            {
                CurrentTurn = new Turn();
                CurrentTurn.Player = CurrentPlayer;
                CurrentTurn.TurnIndex = CurrentTurnIndex;
                game.Turns.Add(CurrentTurn);
            }
        }
        private void TryHandleCurrentPlayerTag(Tag tag)
        {
            if (string.Equals(tag.Name, "CURRENT_PLAYER") &&
                string.Equals(tag.Value, "1"))
            {
                CurrentPlayer = tag.Entity.Name;
            }
        }
        private void TryHandleTurnTag(Tag tag)
        {
            if (string.Equals(tag.Name, "TURN"))
            {
                CurrentTurnIndex = int.Parse(tag.Value);
            }
        }

        private void Read()
        {
            this.lines = File.ReadAllLines(fileName);
        }

        private Entity ParseDraw(string line)
        {
            var cardRegex =
                new Regex(
                    @"TRANSITIONING card (\[name=(?<entityname>[\w\s\dа-яА-Я_-]+)\sid=(?<entityid>\d+)\szone=(?<entityzone>\w+)\szonePos=(?<entityzonepos>\d)\scardId=(?<entitycardid>[\w\d_]+)\splayer=(?<entityplayer>\d)\]|\[id=(?<entityid>\d+)\scardId=\stype=INVALID\szone=(?<entityzone>\w*)\szonePos=(?<entityzonepos>\d+)\splayer=(?<entityplayer>\d)\]|(?<entity>.*)) to (?<hand>OPPOSING|FRIENDLY) HAND");
            var match = cardRegex.Match(line);
            var entity = new Entity();
            int entityId;
            if (!string.IsNullOrEmpty(match.Groups["entityid"].Value) &&
               int.TryParse(match.Groups["entityid"].Value, out entityId))
            {
                entity.Id = entityId;
                if (string.IsNullOrEmpty(match.Groups["entityname"].Value))
                {
                    entity.Id = entityId;
                    entity.Zone = match.Groups["entityzone"].Value;
                    entity.ZonePos = int.Parse(match.Groups["entityzonepos"].Value);
                    entity.Player = int.Parse(match.Groups["entityplayer"].Value);
                }
                else
                {
                    entity.Name = match.Groups["entityname"].Value;
                    entity.CardId = match.Groups["entitycardid"].Value;
                    entity.Zone = match.Groups["entityzone"].Value;
                    entity.ZonePos = int.Parse(match.Groups["entityzonepos"].Value);
                    entity.Player = int.Parse(match.Groups["entityplayer"].Value);
                }
            }
            else
            {
                entity.Name = match.Groups["entity"].Value;
            }
            return entity;
        }

        private int HandleActionStart(int lineIndex)
        {
            var actionInfo = GetActionText(lineIndex);
            var content = actionInfo.Item2;
            var action = ParseAction(content);
            HandleAction(action);
            return actionInfo.Item1;
        }

        private void HandleAction(GameAction action)
        {
            TryHandlePlayCardAction(action);
            TryHandleAttackAction(action);
            TryHandleDeathAction(action);
        }
        private void TryHandlePlayCardAction(GameAction action)
        {
            if (action.SubType != "PLAY") return;
            CurrentTurn.Acts.Add(new PlayCardAct(action.Entity.Card));
        }

        private void TryHandleAttackAction(GameAction action)
        {
            if (action.SubType != "ATTACK") return;
            var attackAct = new AttackAct();
            attackAct.Target = action.Target.Card;
            attackAct.Entity = action.Entity.Card;
            CurrentTurn.Acts.Add(attackAct);
        }
        private void TryHandleDeathAction(GameAction action)
        {
            if (action.SubType != "DEATHS") return;
            //TODO DEATH ONLY ONE TAG PARSED
            foreach (var tag in action.Tags)
            {
                if (string.Equals(tag.Name, "ZONE") &&
                    string.Equals(tag.Value, "GRAVEYARD"))
                {
                    var deathAct = new DeathAct();
                    deathAct.Card = tag.Entity.Card;
                    CurrentTurn.Acts.Add(deathAct);
                }
            }

        }

        private Tuple<int, string> GetActionText(int lineIndex)
        {
            int actionEndNeeded = 1;
            var currentIndex = lineIndex;
            var builder = new StringBuilder();
            while (actionEndNeeded > 0)
            {
                builder.Append(lines[currentIndex]);
                if (lines[currentIndex].Contains("ACTION_END"))
                    actionEndNeeded--;
                currentIndex++;
            }
            return new Tuple<int, string>(currentIndex, builder.ToString());
        }
        private GameAction ParseAction(string actionString)
        {
            var action = ParseActionHeader(actionString);
            action.Tags = ParseTags(actionString);

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
                //if (tag.IsConsistent && !ignoreTag.Contains(tag.Name))
                tags.Add(tag);
            }
            return tags;
        }
        private GameAction ParseActionHeader(string actionString)
        {
            var action = new GameAction();
            var headerRegex =
                new Regex(@"ACTION_START\sEntity=(\[name=(?<entityname>[\w\s\dа-яА-Я_-]+)\sid=(?<entityid>\d+)\szone=(?<entityzone>\w+)\szonePos=(?<entityzonepos>\d)\scardId=(?<entitycardid>[\w\d_]+)\splayer=(?<entityplayer>\d)\]|(?<entityglobalname>[\w\s\d-_]*))\sSubType=(?<subtype>\w+)\sIndex=(?<index>[-\d]+)\sTarget=(\[name=(?<targetname>[\w\s\dа-яА-Я_-]+)\sid=(?<targetid>\d+)\szone=(?<targetzone>\w+)\szonePos=(?<targetzonepos>\d)\scardId=(?<targetcardid>[\w\d_]+)\splayer=(?<targetplayer>\d)\]|(?<targetglobalname>[\w\s\d-_]*))");
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
        private Tag ParseTag(string tagString)
        {
            var tag = new Tag();
            var tagRegex = new Regex(@"Entity=(\[name=(?<entityname>[\w\s\dа-яА-Я_-]+)\sid=(?<entityid>\d+)\szone=(?<entityzone>\w+)\szonePos=(?<entityzonepos>\d)\scardId=(?<entitycardid>[\w\d_]+)\splayer=(?<entityplayer>\d)\]|\[id=(?<xentityid>\d+)\scardId=\stype=INVALID\szone=(?<xentityzone>\w*)\szonePos=(?<xentityzonepos>\d+)\splayer=(?<xentityplayer>\d)\]|(?<entity>[\w\s\d_-]*)) tag=(?<tag>[\w\s\d_-]*) value=(?<value>[\w\s\d_-]*)");
            var match = tagRegex.Match(tagString);
            tag.Entity = new Entity();
            tag.IsConsistent = true;
            int entityId;
            if (!string.IsNullOrEmpty(match.Groups["entityid"].Value) &&
                int.TryParse(match.Groups["entityid"].Value, out entityId))
            {
                tag.Entity.Id = entityId;
                tag.Entity.Name = match.Groups["entityname"].Value;
                tag.Entity.CardId = match.Groups["entitycardid"].Value;
                tag.Entity.Zone = match.Groups["entityzone"].Value;
                tag.Entity.ZonePos = int.Parse(match.Groups["entityzonepos"].Value);
                tag.Entity.Player = int.Parse(match.Groups["entityplayer"].Value);
            }
            else if (!string.IsNullOrEmpty(match.Groups["xentityid"].Value) &&
                int.TryParse(match.Groups["xentityid"].Value, out entityId))
            {
                tag.Entity.Id = entityId;
                tag.Entity.Zone = match.Groups["xentityzone"].Value;
                tag.Entity.ZonePos = int.Parse(match.Groups["xentityzonepos"].Value);
                tag.Entity.Player = int.Parse(match.Groups["xentityplayer"].Value);
            }
            else
            {
                tag.Entity.Name = match.Groups["entity"].Value;
            }
            tag.Name = match.Groups["tag"].Value;
            tag.Value = match.Groups["value"].Value;

            return tag;
        }
    }
}
