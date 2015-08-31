using System.Linq;

namespace HSRecord.Core.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;

    using Acts;
    using Entities;

    public class HSParser
    {
        private string fileName;
        private string[] lines;
        private Game game;
        private Turn CurrentTurn;
        private string CurrentPlayer;
        private int CurrentTurnIndex;
        private bool isMulliganDone;
        private int mulliganFinisherIndex = 0;
        public Game ParseGame(string fileName)
        {
            this.fileName = fileName;
            return this.Parse();
        }

        public Game Parse()
        {
            this.Read();
            this.game = new Game();
            for (var i = 0; i < this.lines.Length; i++)
            {
                var line = this.lines[i];
                //if (line.Contains("TRANSITIONING"))
                //    i = HandleTransitioning(i);
                if (line.Contains("ACTION_START"))
                    i = HandleActionStart(i);
                else if (line.Contains("TAG_CHANGE"))
                    i = HandleTagChange(i);
            }

            return this.game;
        }
        private int HandleTransitioning(int lineIndex)
        {
            if (this.CurrentTurn != null)
            {
                if (this.lines[lineIndex].EndsWith("HAND"))
                {
                    var entity = ParseDraw(this.lines[lineIndex]);
                    if (!isMulliganDone)
                        return lineIndex;
                    this.CurrentTurn.Acts.Add(new DrawCardAct(entity.Card));
                }
            }

            return lineIndex;
        }

        private int HandleTagChange(int lineIndex)
        {
            var tag = ParseTag(this.lines[lineIndex]);
            HandleTagChange(tag);
            return lineIndex;
        }

        private void HandleTagChange(Tag tag)
        {
            TryHandleMulliganTag(tag);
            TryHandleTurnTag(tag);
            TryHandleStepTag(tag);
            TryHandleCurrentPlayerTag(tag);

        }

        private void TryHandleMulliganTag(Tag tag)
        {
            if (string.Equals(tag.Name, "MULLIGAN_STATE") &&
                string.Equals(tag.Value, "DONE"))
            {
                mulliganFinisherIndex++;
                if (mulliganFinisherIndex == 2)
                    this.isMulliganDone = true;
            }
        }

        private void TryHandleStepTag(Tag tag)
        {
            if (!isMulliganDone)
                return;
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
                    @"TRANSITIONING card (\[name=(?<entityname>[\w\s\dа-яА-Я_-]+)\sid=(?<entityid>\d+)\szone=(?<entityzone>\w+)\szonePos=(?<entityzonepos>\d)\scardId=(?<entitycardid>[\w\d_]+)\splayer=(?<entityplayer>\d)\]|\[id=(?<entityid_enemy>\d+)\scardId=\stype=INVALID\szone=(?<entityzone_enemy>\w*)\szonePos=(?<entityzonepos_enemy>\d+)\splayer=(?<entityplayer_enemy>\d)\]|(?<entity>.*) to (?<hand>OPPOSING|FRIENDLY) HAND)");
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
            if (match.Groups["hand"].Value == "FRIENDLY")
                Console.WriteLine("{0}:{1}", CurrentTurnIndex, entity.Name);
            return entity;
        }

        private int HandleActionStart(int lineIndex)
        {
            var actionInfo = GetActionText(lineIndex);
            var content = actionInfo.Item2;
            var action = ParseAction(content, lineIndex, actionInfo.Item1);

            HandleAction(action);
            return actionInfo.Item1;
        }

        private void HandleAction(GameAction action)
        {
            TryHandlePlayCardAction(action);
            TryHandleAttackAction(action);
            TryHandleDeathAction(action);
            TryHandleShowEntity(action);
            TryHandleOtherAction(action);
        }

        private void TryHandleShowEntity(GameAction action)
        {
            if (this.CurrentTurn == null)
                return;
            if (CurrentTurn.TurnIndex == 11)
            {

            }
            for (var i = 0; i < action.ShowEntities.Count; i++)
            {
                var newZone = action.ShowEntities[i].Tags.FirstOrDefault(x => x.Name == "ZONE");
                Act act = null;
                if (newZone!= null && string.Equals("PLAY", newZone.Value))
                    act = new PlayCardAct(action.ShowEntities[i].Entity.Card);
                else
                    act = new DrawCardAct(action.ShowEntities[i].Entity.Card);

                this.CurrentTurn.Acts.Add(act);
            }
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

        private void TryHandleOtherAction(GameAction action)
        {
            foreach (var tag in action.Tags)
            {
                HandleTagChange(tag);
            }
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

        private GameAction ParseAction(string actionString, int from, int to)
        {
            var action = ParseActionHeader(actionString);
            action.Tags = ParseTags(actionString);
            action.ShowEntities = ParseShowEntities(actionString, from, to);
            return action;
        }

        private int HandleShowEntity(int lineIndex)
        {
            var endLineIndex = 0;
            var currentLineIndex = lineIndex;
            while (endLineIndex == 0)
            {
                if (this.lines[currentLineIndex].Contains("TAG_CHANGE"))
                    endLineIndex = currentLineIndex;

                currentLineIndex++;
            }
            var body = string.Empty;
            for (var i = lineIndex; i < endLineIndex; i++)
                body += lines[i];

            var showEntities = ParseShowEntities(body, lineIndex, endLineIndex);
            if (this.CurrentTurn != null)
                for (var i = 0; i < showEntities.Count; i++)
                {
                    this.CurrentTurn.Acts.Add(new DrawCardAct(showEntities[i].Entity.Card));
                }
            return endLineIndex;
        }

        private List<ShowEntity> ParseShowEntities(string actionString, int from, int to)
        {
            var entities = new List<ShowEntity>();
            var actionLines = actionString.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            for (var i = from; i < to; i++)
            {
                if (lines[i].Contains("SHOW_ENTITY - Updating"))
                {
                    var j = i + 1;
                    for (; j < to || lines[j].Contains("TAG_CHANGE"); j++)
                    {
                    }



                    entities.Add(ParseShowEntity(i, j));
                }
            }
            return entities;
        }

        private List<ShowEntity> ParseShowEntities2(string actionString)
        {
            var regex =
                new Regex(
                //@"SHOW_ENTITY - Updating Entity=\[id=(?<id>\d+) cardId= type=(.*) zone=(?<zone>HAND|DECK|PLAY) zonePos=(?<zonepos>\d+) player=(?<player>\d+)] CardID=(?<cardid>[\w\d_]+)([\s\S.]*?)TAG_CHANGE");
                    @"SHOW_ENTITY - Updating Entity=\[.*] CardID=(?<cardid>[\w\d_]+)([\s\S.]*?)TAG_CHANGE");
            var matches = regex.Matches(actionString);

            var showEntities = new List<ShowEntity>();
            foreach (Match match in matches)
            {
                if (CurrentTurn != null && CurrentTurn.TurnIndex == 11)
                {

                }
                var entity = ParseShowEntity2(match);
                entity.Tags = this.ParseShowEntityTags(match.Value);
                showEntities.Add(entity);
            }
            return showEntities;
        }

        private ShowEntity ParseShowEntity(int from, int to)
        {
            var entity = new ShowEntity();
            var showEntityLine = lines[from];
            var regex = new Regex(@"CardID=(?<cardid>[\w\d_]+)");
            var match = regex.Match(lines[from]);
            entity.Entity.CardId = match.Groups["cardid"].Value;

            var tagRegex = new Regex(@"tag=(?<name>\w+)\svalue=(?<value>[\w\d_]+)");
            for (var i = from; i < to; i++)
            {
                var tagMatch = tagRegex.Match(lines[i]);
                if (tagMatch.Success)
                    entity.Tags.Add(new Tag()
                    {
                        Name = tagMatch.Groups["name"].Value,
                        Value = tagMatch.Groups["value"].Value
                    });
            }

            return entity;
        }
        private ShowEntity ParseShowEntity2(Match match)
        {
            var entity = new ShowEntity();
            //entity.Entity.Id = Int16.Parse(match.Groups["id"].Value);
            //entity.Entity.Zone = match.Groups["zone"].Value;
            //entity.Entity.ZonePos = Int16.Parse(match.Groups["zonepos"].Value);
            //entity.Entity.Player = Int16.Parse(match.Groups["player"].Value);
            entity.Entity.CardId = match.Groups["cardid"].Value;
            return entity;
        }
        private List<Tag> ParseShowEntityTags(string showEntityBody)
        {
            var regex = new Regex(@"tag=(?<tagname>[\w_]+)\s+value=(?<tagvalue>[\w\d_]+)");
            var tagMatches = regex.Matches(showEntityBody);
            var tags = new List<Tag>();
            foreach (Match tagMatch in tagMatches)
            {
                tags.Add(new Tag()
                {
                    Name = tagMatch.Groups["tagname"].Value,
                    Value = tagMatch.Groups["tagvalue"].Value
                });
            }
            return tags;
        }
        private List<Tag> ParseTags(string actionString)
        {
            var tagRegex = new Regex(@"TAG_CHANGE\sEntity=(\[name=(?<entityname>[\w\s\dа-яА-Я_-]+)\sid=(?<entityid>\d+)\szone=(?<entityzone>\w+)\szonePos=(?<entityzonepos>\d)\scardId=(?<entitycardid>[\w\d_]+)\splayer=(?<entityplayer>\d)\]|\[id=(?<xentityid>\d+)\scardId=\stype=INVALID\szone=(?<xentityzone>\w*)\szonePos=(?<xentityzonepos>\d+)\splayer=(?<xentityplayer>\d)\]|(?<entity>[\w\s\d_-]*)) tag=(?<tag>[\w\s\d_-]*) value=(?<value>[\w\s\d_-]*)");
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
            tag.Value = tag.Value.Trim();
            return tag;
        }
    }
}
