using AutoFixture;
using AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ValidCertificateAttribute : AutoDataAttribute
    {
        public ValidCertificateAttribute() : base(() =>
        {
            var fixture = new Fixture();
            fixture.Customizations.Add(new CertificateSpecimenBuilder());
            return fixture;
        })
        {
        }
    }
}
