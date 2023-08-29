// Enums used in MLA-Helper Settings.
namespace MLAHelper {
    public static class MLAHelperSettingsEnums 
    {
        // When to start visualization
        public enum VisualizeOn {
            Start = 0,
            Awake = 10,
            Never = 20
        }

        // Whether to focus on discrete_actions or deterministic_discrete_actions
        public enum ActionFocus {
            discrete_actions = 0,
            deterministic_discrete_actions = 10
        }
    }
}