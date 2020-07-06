using System.Reflection;
using System.Web.Mvc;
using StyleCop;
using System.Linq;
using StyleCop.CSharp;
using System.Runtime.Serialization;

namespace StaticCodeAnalyzer.StyleCop
{
    [SourceAnalyzer(typeof(CsParser))]
    public class ClassRule : SourceAnalyzer
    {
        const string ruleName = "ClassRule";
        public override void AnalyzeDocument(CodeDocument document)
        {
            CsDocument csDocument = (CsDocument)document;
            csDocument.WalkDocument(new CodeWalkerElementVisitor<object>(this.VisitElement));
        }
        private bool VisitElement(object element, CsElement parentElement, object context)
        {

            var classElement = element as Class;
            if (classElement != null)
            {
                if (!classElement.GetType().IsPublic)
                {
                    this.AddViolation((ICodeElement)element, ruleName);
                    return false;
                }

                if (!classElement.GetType().GetCustomAttributes(true)?.Any(a => a.GetType().Equals(typeof(DataContractAttribute))) ?? false)
                {
                    this.AddViolation((ICodeElement)element, ruleName);
                    return false;
                }
               
                if (classElement.GetType().GetProperty("Id")!= null && classElement.GetType().GetProperty("Name") != null)
                {
                    if (!(classElement.GetType().GetProperty("Id").GetSetMethod(true).IsPublic && classElement.GetType().GetProperty("Name").GetSetMethod(true).IsPublic))
                    {
                        this.AddViolation((ICodeElement)element, ruleName);
                        return false;
                    }
                 }

            }
            return true;

        }
      
    }
}