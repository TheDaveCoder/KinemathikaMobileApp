using System.Collections.Generic;

public interface IConceptStateManager
{
    void LoadProblemHeaderMetadata(ProblemHeaderMetadata problemHeaderMetadata);
    ProblemHeaderMetadata GetProblemHeaderMetadata();
}