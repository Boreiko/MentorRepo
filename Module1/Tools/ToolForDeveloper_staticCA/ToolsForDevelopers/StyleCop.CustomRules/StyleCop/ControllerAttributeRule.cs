using System.Reflection;
using System.Web.Mvc;
using StyleCop;
using System.Linq;
using StyleCop.CSharp;

namespace StaticCodeAnalyzer.StyleCop
{
    [SourceAnalyzer(typeof(CsParser))]
    public class ControllerAttributeRule : SourceAnalyzer
    {
        const string ruleName = "ControllerAttributeRule";
        public override void AnalyzeDocument(CodeDocument document)
        {
            CsDocument csDocument = (CsDocument)document;
            csDocument.WalkDocument(new CodeWalkerElementVisitor<object>(this.VisitElement));
        }
        private bool VisitElement(object element, CsElement parentElement, object context)
        {

            var controllerElement = element as Controller;
            if(controllerElement != null)
            {
                if (!controllerElement.GetType().GetCustomAttributes(true)?.Any(a => a.GetType().Equals(typeof(Attribute))) ?? false)
               
                    this.AddViolation((ICodeElement)element, ruleName);
                return false;
            }
            return true;

        }
      
    }
}