using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClippingManager
{
    /// <summary>
    /// These enums have to be modified whenever a new Clipping type has to be recognized.
    /// </summary>

    public enum ClippingTypeEnum
    {
        Highlight,
        Note,
        Bookmark,
        Subrayado,
        Notas,
        Marcador,
        NotRecognized,
        NoReconocido,
    }
}
