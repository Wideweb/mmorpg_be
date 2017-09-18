using System.Collections.Generic;

namespace Game.Api.Game.Dto
{
    public class MapDto
    {
        public IEnumerable<IEnumerable<MapCellDto>> Cells { get; set; }
    }
}
