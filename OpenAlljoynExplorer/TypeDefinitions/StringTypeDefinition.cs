using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceProviders;

namespace OpenAlljoynExplorer.TypeDefinitions
{
    /// <summary>
    /// Alternative for
    /// 
    /// var stringTemplateType = AllJoynTypeDefinition.CreateTypeDefintions("s")[0];
    ///
    /// </summary>
    public class AlljoynTypeDefinition : ITypeDefinition
    {
        private TypeId mType;
        private AlljoynTypeDefinition(TypeId type) {
            mType = type;
        }

        public static AlljoynTypeDefinition InstanceByType(TypeId type) {
            switch (type)
            {
                case TypeId.Boolean:
                    return Boolean;
                case TypeId.Double:
                    return Double;
                case TypeId.Int32:
                    return Int32;
                case TypeId.Int16:
                    return Int16;
                case TypeId.Uint16:
                    return Uint16;
                case TypeId.String:
                    return String;
                case TypeId.Uint64:
                    return Uint64;
                case TypeId.Uint32:
                    return Uint32;
                case TypeId.Int64:
                    return Int64;
                case TypeId.Uint8:
                    return Uint8;
            }
            throw new NotSupportedException($"type={type}");
        }

        public static AlljoynTypeDefinition TypeInstanceByArrayType(TypeId arrayType)
        {
            switch (arrayType)
            {
                case TypeId.BooleanArray:
                    return Boolean;
                case TypeId.DoubleArray:
                    return Double;
                case TypeId.Int32Array:
                    return Int32;
                case TypeId.Int16Array:
                    return Int16;
                case TypeId.Uint16Array:
                    return Uint16;
                case TypeId.StringArray:
                    return String;
                case TypeId.Uint64Array:
                    return Uint64;
                case TypeId.Uint32Array:
                    return Uint32;
                case TypeId.Int64Array:
                    return Int64;
                case TypeId.Uint8Array:
                    return Uint8;
                case TypeId.ObjectPathArray:
                    return ObjectPath;
            }
            throw new NotSupportedException($"arrayType={arrayType}");
        }

        public static readonly AlljoynTypeDefinition Boolean = new AlljoynTypeDefinition(TypeId.Boolean);
        public static readonly AlljoynTypeDefinition Uint8 = new AlljoynTypeDefinition(TypeId.Uint8);
        public static readonly AlljoynTypeDefinition Uint16 = new AlljoynTypeDefinition(TypeId.Uint16);
        public static readonly AlljoynTypeDefinition Uint32 = new AlljoynTypeDefinition(TypeId.Uint32);
        public static readonly AlljoynTypeDefinition Uint64 = new AlljoynTypeDefinition(TypeId.Uint64);
        public static readonly AlljoynTypeDefinition Int16 = new AlljoynTypeDefinition(TypeId.Int16);
        public static readonly AlljoynTypeDefinition Int32 = new AlljoynTypeDefinition(TypeId.Int32);
        public static readonly AlljoynTypeDefinition Int64 = new AlljoynTypeDefinition(TypeId.Int64);
        public static readonly AlljoynTypeDefinition Double = new AlljoynTypeDefinition(TypeId.Double);
        public static readonly AlljoynTypeDefinition Signature = new AlljoynTypeDefinition(TypeId.Signature);
        public static readonly AlljoynTypeDefinition String = new AlljoynTypeDefinition(TypeId.String);
        public static readonly AlljoynTypeDefinition ObjectPath = new AlljoynTypeDefinition(TypeId.ObjectPath);
        public static readonly AlljoynTypeDefinition Variant = new AlljoynTypeDefinition(TypeId.Variant);

        //public static readonly AlljoynTypeDefinition Struct = new AlljoynTypeDefinition(TypeId.Struct);
        //public static readonly AlljoynTypeDefinition Variant = new AlljoynTypeDefinition(TypeId.Variant);

        public ITypeDefinition ValueType => null;

        public ITypeDefinition KeyType => null;

        public IReadOnlyList<ITypeDefinition> Fields => null;

        public TypeId Type => mType;
    }
}
