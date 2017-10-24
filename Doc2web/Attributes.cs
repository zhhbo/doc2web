using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web
{
    /// <summary>
    /// Attribute that register a method as a hook for the initialization of an ConversionEngine.
    /// Make sure that the method with this attribute takes a ContainerBuilder as unique parameter.
    /// </summary>
    public class InitializeEngineAttribute : Attribute { }

    /// <summary>
    /// Attribute that register a method as a hook for the pre-processing of a conversion task.
    /// Make sure that the method with this attribute takes a IGlobalContext as unique paremeter.
    /// </summary>
    public class PreProcessingAttribute : Attribute { }

    /// <summary>
    /// Attribute that register a method as a hook for processing elements.
    /// Make sure that the method with this attributes takes a IElementContext as first parameters 
    /// and the element as seccond parameter. The element's class must extends OpenXmlElement.
    /// </summary>
    public class ElementProcessingAttribute : Attribute { }

    /// <summary>
    /// Attribute that register a method as a hook for the post-processing of a conversion task. 
    /// Make sure that the method with this attribute takes a IGlobalContext as unique parameter.
    /// </summary>
    public class PostProcessingAttribute : Attribute { }
}
