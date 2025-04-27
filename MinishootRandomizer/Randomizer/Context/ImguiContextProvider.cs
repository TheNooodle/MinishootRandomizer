using UnityEngine;

namespace MinishootRandomizer;

public class ImguiContextProvider : IRandomizerContextProvider
{
    private readonly IObjectFinder _objectFinder;
    private readonly ILogger _logger = new NullLogger();

    public ImguiContextProvider(IObjectFinder objectFinder, ILogger logger = null)
    {
        _objectFinder = objectFinder;
        _logger = logger ?? new NullLogger();
    }

    public RandomizerContext GetContext()
    {
        try
        {
            GameObject imguiObject = _objectFinder.FindObject(new ByName("RandomizerImgui"));
            ImguiContextComponent imguiContext = imguiObject.GetComponent<ImguiContextComponent>();

            return imguiContext.GetContext();
        }
        catch (ObjectNotFoundException)
        {
            _logger.LogError("Imgui object not found, returning vanilla context");

            return new VanillaContext();
        }
    }
}
