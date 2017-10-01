using System.Collections.Generic;

namespace Game.Api.Dto
{
    public class MapDto
    {
        public IEnumerable<IEnumerable<MapCellDto>> Cells { get; set; }
    }
}
