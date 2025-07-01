using System.Collections.Generic;
using System.Linq;

public class ConceptStateManager : IConceptStateManager
{
    private ProblemHeaderMetadata _problemHeaderMetadata;

    public void LoadProblemHeaderMetadata(ProblemHeaderMetadata problemHeaderMetadata)
    {
        _problemHeaderMetadata = problemHeaderMetadata;
    }

    public ProblemHeaderMetadata GetProblemHeaderMetadata()
    {
        return _problemHeaderMetadata;
    }
}
