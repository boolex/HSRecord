class LogEvent{
	EventTypeResolver resolver;
	
	LogEvent(string content){
		this.Content = content;
	}
	public string Content {get;set;}
	
	public abstract GameEvent Parse();
}

class ActionEvent : LogEvent{}
class TagEvent: LogEvent{}

public class EventTypeResolver
{
	public LogEvent Resolve(LogEvent)
	{
	}
}

foreach(var logEvent in logEvents){
	var event = resolver.resolve(logEvent);
	event.parse();
}
