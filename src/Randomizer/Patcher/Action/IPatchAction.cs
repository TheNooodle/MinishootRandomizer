namespace MinishootRandomizer;

public interface IPatchAction
{
    void Patch();
    void Unpatch();
    void Dispose();
}
