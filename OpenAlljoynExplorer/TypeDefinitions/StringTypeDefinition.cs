using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceProviders;

namespace OpenAlljoynExplorer.TypeDefinitions
{
    public class StringTypeDefinition : ITypeDefinition
    {
        private StringTypeDefinition() { }

        private static readonly StringTypeDefinition instance = new StringTypeDefinition();

        public static ITypeDefinition Instance => instance;

        public ITypeDefinition ValueType => null;

        public ITypeDefinition KeyType => null;

        public IReadOnlyList<ITypeDefinition> Fields => null;

        public TypeId Type => TypeId.String;
    }
}
