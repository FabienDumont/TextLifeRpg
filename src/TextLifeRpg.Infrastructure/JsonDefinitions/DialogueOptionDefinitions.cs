namespace TextLifeRpg.Infrastructure.JsonDefinitions;

public class DialogueOptionDefinition
{
  public required string Label { get; set; }
  public List<DialogueSpokenTextDefinition> SpokenTexts { get; set; } = [];
  public List<DialogueResultDefinition> Results { get; set; } = [];
}

public class DialogueSpokenTextDefinition
{
  public required string Text { get; set; }
  public List<DialogueConditionDefinition> Conditions { get; set; } = [];
}

public class DialogueResultDefinition
{
  public bool EndsDialogue { get; set; }
  public List<DialogueResultSpokenTextDefinition> ResultSpokenTexts { get; set; } = [];
  public List<DialogueResultNarrationDefinition> ResultNarrations { get; set; } = [];
}

public class DialogueResultSpokenTextDefinition
{
  public required string Text { get; set; }
  public List<DialogueConditionDefinition> Conditions { get; set; } = [];
}

public class DialogueResultNarrationDefinition
{
  public required string Text { get; set; }
  public List<DialogueConditionDefinition> Conditions { get; set; } = [];
}

public class DialogueConditionDefinition
{
  public string? Trait { get; set; }
  public ConditionComparisonDefinition? ActorRelationshipValue { get; set; }

  public ConditionComparisonDefinition? ActorEnergy { get; set; }
  // Extendable with TargetTrait, TimeOfDay, etc.
}
