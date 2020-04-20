using System;
using System.Collections.Generic;
using System.Text;

namespace ENTIDAD
{
    public class Resultado
    {
        string error = String.Empty;
        Boolean todoCorrecto = false;

        public string Error { get => error; set => error = value; }
        public bool TodoCorrecto { get => todoCorrecto; set => todoCorrecto = value; }
    }
}
