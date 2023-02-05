﻿using OALProgramControl;

namespace AnimArch.Visualization.Diagrams
{
    public static class CDClassEditor
    {
        public static CDClass CreateNode(Class newClass)
        {
            CDClass cdClass = null;
            var i = 0;
            var baseName = newClass.Name;
            while (cdClass == null)
            {
                newClass.Name = baseName + (i == 0 ? "" : i.ToString());
                cdClass = OALProgram.Instance.ExecutionSpace.SpawnClass(newClass.Name);
                i++;
                if (i > 1000)
                {
                    break;
                }
            }

            return cdClass;
        }

        private static CDMethod CreateCdMethodFromMethod(CDClass cdClass, Method method)
        {
            return new CDMethod(cdClass, method.Name, EXETypes.ConvertEATypeName(method.ReturnValue));
        }

        public static void AddMethod(ClassInDiagram classInDiagram, Method method)
        {
            var cdMethod = CreateCdMethodFromMethod(classInDiagram.ClassInfo, method);

            if (method.arguments != null)
                AddParameters(method, cdMethod);

            classInDiagram.ClassInfo.AddMethod(cdMethod);
        }


        private static void AddParameters(Method method, CDMethod cdMethod)
        {
            foreach (var argument in method.arguments)
            {
                var tokens = argument.Split(' ');
                var type = tokens[0];
                var name = tokens[1];

                cdMethod.Parameters.Add(new CDParameter { Name = name, Type = EXETypes.ConvertEATypeName(type) });
            }
        }

        public static void UpdateMethod(ClassInDiagram classInDiagram, string oldMethod, Method newMethod)
        {
            var index = classInDiagram.ClassInfo.Methods.FindIndex(x => x.Name == oldMethod);
            var newCdMethod = CreateCdMethodFromMethod(classInDiagram.ClassInfo, newMethod);
            classInDiagram.ClassInfo.Methods[index] = newCdMethod;
        }

        private static CDAttribute CreateCdAttributeFromAttribute(Attribute attribute)
        {
            return new CDAttribute(attribute.Name, EXETypes.ConvertEATypeName(attribute.Type));
        }
        public static void AddAttribute(ClassInDiagram classInDiagram, Attribute attribute)
        {
            var cdAttribute = CreateCdAttributeFromAttribute(attribute);
            classInDiagram.ClassInfo.AddAttribute(cdAttribute);
        }

        public static void UpdateAttribute(ClassInDiagram classInDiagram, string oldAttribute, Attribute newAttribute)
        {
            var index = classInDiagram.ClassInfo.Attributes.FindIndex(x => x.Name == oldAttribute);
            var newCdAttribute = CreateCdAttributeFromAttribute(newAttribute);
            classInDiagram.ClassInfo.Attributes[index] = newCdAttribute;
        }
    }
}