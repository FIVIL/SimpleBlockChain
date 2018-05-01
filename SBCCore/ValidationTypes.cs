using System;
using System.Collections.Generic;
using System.Text;

namespace NoobChain
{
    public enum ValidationTypes
    {
        Valid,
        NotValidCurrent,
        NotValidPervious,
        HasntMinedYet,
        TransactionSignatureIsInvalide,
        TransactionInputsValueandOutputsValuearentEqual,
        TransactionInputsisMissing,
        TransactionInputsValueisInvalide,
        WrongReciepient,
        //change is left out value
        ChangeSenderInvalide
    }
}
