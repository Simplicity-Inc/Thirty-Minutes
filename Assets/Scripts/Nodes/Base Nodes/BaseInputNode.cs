


public abstract class BaseInputNode : BaseNode {

    protected string nodeResult = "None";

    public virtual string getResult() { return nodeResult; }
    public override void DrawCurves() { }
}
