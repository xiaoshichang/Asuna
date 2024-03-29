
csharp_template = """
*************************************************
!!! Auto generated code from jinja2 render engine!
!!! Do not modify this file manually!
************************************************

using System;
using System.Collections;
using System.Collections.Generic;

namespace DesignModel
{
    public class {{class_name}}
    {
        {% for field in fields %}
        // {{ field.field_desc }}
        public {{ field.convert_to_csharp_type() }} {{field.field_name }};
        {% endfor %}
    }
}
"""

