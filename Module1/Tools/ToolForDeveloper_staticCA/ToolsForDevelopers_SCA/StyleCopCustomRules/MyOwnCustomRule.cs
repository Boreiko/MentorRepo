using System.Reflection;
using StyleCop;
using System.Linq;
using StyleCop.CSharp;
using System.Runtime.Serialization;
using System;

namespace StyleCopCustomRules
{
    [SourceAnalyzer(typeof(CsParser))]
    public class MyOwnCustomRule : SourceAnalyzer
    {
        public override void AnalyzeDocument(CodeDocument document)
        {
            CsDocument csDocument = (CsDocument)document;
            csDocument.WalkDocument(new CodeWalkerElementVisitor<object>(this.VisitElement));
        }
        private bool VisitElement(CsElement element, CsElement parentElement, object context)
        {
            var controllerElement = element as Class;

            if (controllerElement != null && controllerElement.BaseClass.Contains("Controller"))
            {
                if (!controllerElement.Name.EndsWith("Controller"))
                    AddViolation(element, "SystemWebMvcControllerMustHaveSuffixController");

                if (!controllerElement.Attributes.Any(t => t.Text.Contains("Authorize")))
                {
                    var publicMethods = element.ChildElements.Where(x => x.ElementType == ElementType.Method && x.AccessModifier == AccessModifierType.Public).ToList();

                    if (publicMethods.Count() != 0)
                    {
                        foreach (var item in publicMethods)
                        {
                            if (item.Attributes.Any(a => a.Text.Contains("Authorize"))) { }

                            else
                            {
                                AddViolation(element, "SystemWebMvcControllerMustHaveAttribute");
                                break;
                            }
                        }
                    }
                }

            }
                var clsItem = element as Class;
          
                if (clsItem != null)
                {   
                
                if (clsItem.AccessModifier != AccessModifierType.Public)
                    AddViolation(element, "ClassMustBePublic");
                        
                if (!clsItem.Attributes.Any( x=> x.ChildTokens.Any(t => t.Text.Contains("DataContract"))))
                    AddViolation(parentElement, "ClassMustHaveAttribute");
 
                var properties = clsItem.ChildElements.Where(x => x.ElementType == ElementType.Property);

                    var idProp = properties.FirstOrDefault(x => x.Declaration.Name == "Id");

                    if (idProp == null || idProp.AccessModifier != AccessModifierType.Public)
                        AddViolation(element, "ClassMustHavePublicId");

                    var nameProp = properties.FirstOrDefault(x => x.Declaration.Name == "Name");

                    if (nameProp == null || nameProp.AccessModifier != AccessModifierType.Public)
                        AddViolation(element, "ClassMustHavePublicName");
                }

            return true;
        }
              
    }
}
    

