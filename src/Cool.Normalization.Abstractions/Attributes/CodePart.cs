using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization
{
    /// <summary>
    /// Specifics which part is code will be placed to.
    /// </summary>
    [Flags]
    public enum CodePart
    {
        /// <summary>
        /// Default part. Which means :
        /// when <see cref="ICodeAttribute"/> appears on :
        /// application service class => <see cref="Service"/> ,
        /// input dto class => <see cref="Level"/> ,
        /// output dto class => <see cref="Detail"/> ,
        /// method => <see cref="Api"/>,
        /// property => <see cref="Detail"/>,
        /// <see cref="NormalizationException"/> => <see cref="Detail"/>
        /// </summary>
        Default = 1 << 0,
        /// <summary>
        /// Level part.This is the 1st part of a full code.
        /// </summary>
        Level = 1 << 1,
        /// <summary>
        /// Service part.This is the 2st part of a full code.
        /// </summary>
        Service = 1 << 2,
        /// <summary>
        /// Api part. This is the 3rd part of a full code.
        /// </summary>
        Api = 1 << 3,
        /// <summary>
        /// Detail part. This is the 4th part, the last part of a full code.
        /// </summary>
        Detail = 1 << 4,






    }
}
