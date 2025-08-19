using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Reflection;

namespace GiacintDllExpo.Lib.Services
{
    internal class StringHelper
    {
        internal static string PathFromArgs(string[] args, byte index)
        {
            if (args.Length <= index)
            {
                throw new ArgumentException($"Argument at index {index} is missing.");
            }

            string path = "";
            for (byte i = index; i < args.Length; i++)
            {
                path = path + ' ' + args[i];
            }

            return path.Trim('\"', ' ');
        }

        internal static string AddTabs(string str, int tabCount = 1)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            string tabs = new string('\t', tabCount);
            return str.Replace("\r\n", $"\r\n{tabs}").Replace("\n", $"\n{tabs}").Insert(0, tabs);
        }

        public static Instruction ParseInstruction(string text, ModuleDefinition module)
        {
            var parts = text.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
                throw new Exception("Empty instruction");

            var opcodeField = typeof(OpCodes).GetFields(BindingFlags.Public | BindingFlags.Static)
                .FirstOrDefault(f => f.Name.Equals(parts[0], StringComparison.OrdinalIgnoreCase));

            if (opcodeField == null)
                throw new Exception("Unknown OpCode: " + parts[0]);

            var op = (OpCode)opcodeField.GetValue(null);

            if (parts.Length == 1)
                return Instruction.Create(op);

            var operand = parts[1];

            switch (op.OperandType)
            {
                case OperandType.InlineString:
                    return Instruction.Create(op, operand);

                case OperandType.InlineI:
                case OperandType.ShortInlineI:
                    return Instruction.Create(op, int.Parse(operand));

                case OperandType.InlineI8:
                    return Instruction.Create(op, long.Parse(operand));

                case OperandType.ShortInlineR:
                    return Instruction.Create(op, float.Parse(operand));

                case OperandType.InlineR:
                    return Instruction.Create(op, double.Parse(operand));

                case OperandType.InlineMethod:
                    {
                        var method = module.Types
                            .SelectMany(t => t.Methods)
                            .FirstOrDefault(m => m.FullName == operand);
                        if (method == null)
                            throw new Exception("Method not found: " + operand);
                        return Instruction.Create(op, method);
                    }

                case OperandType.InlineField:
                    {
                        var field = module.Types
                            .SelectMany(t => t.Fields)
                            .FirstOrDefault(f => f.FullName == operand);
                        if (field == null)
                            throw new Exception("Field not found: " + operand);
                        return Instruction.Create(op, field);
                    }

                case OperandType.InlineType:
                    {
                        var type = module.Types.FirstOrDefault(t => t.FullName == operand);
                        if (type == null)
                            throw new Exception("Type not found: " + operand);
                        return Instruction.Create(op, type);
                    }

                case OperandType.InlineTok:
                    {
                        var type = module.Types.FirstOrDefault(t => t.FullName == operand);
                        if (type != null)
                            return Instruction.Create(op, type);
                        var field = module.Types.SelectMany(t => t.Fields).FirstOrDefault(f => f.FullName == operand);
                        if (field != null)
                            return Instruction.Create(op, field);
                        var method = module.Types.SelectMany(t => t.Methods).FirstOrDefault(m => m.FullName == operand);
                        if (method != null)
                            return Instruction.Create(op, method);
                        throw new Exception("Token not found: " + operand);
                    }

                case OperandType.ShortInlineVar:
                case OperandType.InlineVar:
                    {
                        if (operand.StartsWith("V_"))
                        {
                            int index = int.Parse(operand.Substring(2));
                            return Instruction.Create(op, (VariableDefinition)module.Types.SelectMany(t => t.Methods).SelectMany(m => m.Body?.Variables ?? Enumerable.Empty<VariableDefinition>()).First(v => v.Index == index));
                        }
                        else
                        {
                            int index = int.Parse(operand);
                            return Instruction.Create(op, (VariableDefinition)module.Types.SelectMany(t => t.Methods).SelectMany(m => m.Body?.Variables ?? Enumerable.Empty<VariableDefinition>()).First(v => v.Index == index));
                        }
                    }

                case OperandType.ShortInlineBrTarget:
                case OperandType.InlineBrTarget:
                    throw new NotSupportedException("Branch targets require context of full method body.");

                default:
                    throw new NotSupportedException($"Unsupported operand type {op.OperandType} for opcode {op}");
            }

        }
    }
}
