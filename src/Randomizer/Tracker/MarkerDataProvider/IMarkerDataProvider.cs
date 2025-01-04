using System.Collections.Generic;

namespace MinishootRandomizer;

public interface IMarkerDataProvider
{
    List<MarkerData> GetMarkerDatas();
}
