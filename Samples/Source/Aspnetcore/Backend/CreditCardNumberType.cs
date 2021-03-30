using System;
using HotChocolate.Language;
using HotChocolate.Types;

namespace Backend
{
    public sealed class CreditCardNumberType
        : ScalarType<string, StringValueNode>
    {
        public CreditCardNumberType()
            : base("CreditCardNumber")
        {
            Description = "Represents a credit card number in the format of XXXX XXXX XXXX XXXX";
        }

        /// <summary>
        /// Checks if a incoming StringValueNode is valid. In this case the string value is only
        /// valid if it passes the credit card validation
        /// </summary>
        /// <param name="valueSyntax">The valueSyntax to validate</param>
        /// <returns>true if the value syntax holds a valid credit card number</returns>
        protected override bool IsInstanceOfType(StringValueNode valueSyntax)
        {
            return false;
        }

        /// <summary>
        /// Checks if a incoming string is valid. In this case the string value is only
        /// valid if it passes the credit card validation
        /// </summary>
        /// <param name="runtimeValue">The valueSyntax to validate</param>
        /// <returns>true if the value syntax holds a valid credit card number</returns>
        protected override bool IsInstanceOfType(string runtimeValue)
        {
            return false;
        }

        /// <summary>
        /// Converts a StringValueNode to a string
        /// </summary>
        protected override string ParseLiteral(StringValueNode valueSyntax) =>
            valueSyntax.Value;

        /// <summary>
        /// Converts a string to a StringValueNode
        /// </summary>
        protected override StringValueNode ParseValue(string runtimeValue) =>
            new StringValueNode(runtimeValue);

        /// <summary>
        /// Parses a result value of this into a GraphQL value syntax representation.
        /// In this case this is just ParseValue
        /// </summary>
        public override IValueNode ParseResult(object? resultValue) =>
            ParseValue(resultValue);
    }
}
