using Unity.VisualScripting.Antlr3.Runtime;

namespace PoliceInterview.Core
{
    public partial class DetectiveStatementProposal
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public GuardrailCheck TheGuardrailCheck { get; set; }

        public string IntervieweeResponse { get; set; }

        public StressScore TheStressScore { get; set; }

        public bool ResponseHasBeenSent { get; set; }

        public DetectiveStatementProposal(int id, string text)
        {
            Id = id;
            Text = text;
            TheGuardrailCheck = null;
            IntervieweeResponse = null;
            TheStressScore = null;
            ResponseHasBeenSent = false;
        }

        public bool IsReadyForInterviewee()
        {
            if (IntervieweeResponse != null) return false;
            if (TheGuardrailCheck ==null) return false;
            if (TheGuardrailCheck.allowed == false) return false;
            if (TheStressScore == null) return false;
            return true;
        }

        public bool IsReadyToPost()
        {
            if (IntervieweeResponse == null) return false;
            if (TheGuardrailCheck == null) return false;
            if (TheGuardrailCheck.allowed == false) return false;
            if (TheStressScore == null) return false;
            return true;
        }

        public override string ToString()
        {
            IntervieweeResponse = null;
            TheStressScore = null;
            ResponseHasBeenSent = false;

            return $"{base.ToString()} Id={Id}, Text={Text}, TheGuardrailCheck={TheGuardrailCheck}, IntervieweeResponse={IntervieweeResponse}, TheStressScore={TheStressScore}, ResponseHasBeenSent={ResponseHasBeenSent}";
        }
    }
}