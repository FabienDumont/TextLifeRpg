namespace TextLifeRpg.Infrastructure.JsonDefinitions;

public class DialogueOptionDefinition
{
  public required string Name { get; set; }
  public required string Label { get; set; }
  public List<DialogueOptionSpokenTextDefinition> SpokenTexts { get; set; } = [];
  public List<DialogueOptionResultDefinition> Results { get; set; } = [];
  public List<DialogueOptionConditionDefinition> Conditions { get; set; } = [];
}

public class DialogueOptionSpokenTextDefinition
{
  public required string Text { get; set; }
  public List<DialogueOptionConditionDefinition> Conditions { get; set; } = [];
}

public class DialogueOptionResultDefinition
{
  public int? TargetRelationshipValueChange { get; set; }
  public string? ActorLearnFact { get; set; }
  public string? ActorTargetSpecialAction { get; set; }
  public List<string>? NextDialogueOptionNames { get; set; }
  public bool EndsDialogue { get; set; }
  public List<DialogueOptionResultSpokenTextDefinition> ResultSpokenTexts { get; set; } = [];
  public List<DialogueOptionResultNarrationDefinition> ResultNarrations { get; set; } = [];
  public List<DialogueOptionConditionDefinition> Conditions { get; set; } = [];
}

public class DialogueOptionResultSpokenTextDefinition
{
  public required string Text { get; set; }
  public List<DialogueOptionConditionDefinition> Conditions { get; set; } = [];
}

public class DialogueOptionResultNarrationDefinition
{
  public required string Text { get; set; }
  public List<DialogueOptionConditionDefinition> Conditions { get; set; } = [];
}

public class DialogueOptionConditionDefinition
{
  public string? ActorHasTrait { get; set; }
  public string? ActorHasntLearnedFact { get; set; }
  public DialogueOptionSpecialConditionDefinition? ActorTargetSpecialCondition { get; set; }
  public ConditionComparisonDefinition? ActorRelationshipValue { get; set; }
  public ConditionComparisonDefinition? TargetRelationshipValue { get; set; }
  public ConditionComparisonDefinition? ActorEnergy { get; set; }
  // Extendable with TargetTrait, TimeOfDay, etc.
}

public class DialogueOptionSpecialConditionDefinition
{
  public required string Label { get; set; }
  public bool Negate { get; set; }
}
