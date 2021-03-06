/*
 * Copyright 2007 - 2009 Marek St�j
 * 
 * This file is part of ImmDoc .NET.
 *
 * ImmDoc .NET is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * ImmDoc .NET is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with ImmDoc .NET; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
 */

using System;
using System.Collections.Generic;
using System.IO;

using Imm.ImmDocNetLib.Documenters;
using System.Diagnostics;

namespace Imm.ImmDocNetLib.MyReflection.MetaClasses
{
    class MyPropertiesOverloadsInfo : MetaClass, IEnumerable<MyPropertyInfo>, ISummarisableMember
    {
        private List<MyPropertyInfo> overloads;
        private MyPropertyInfo propertyWithTheSmallestNumberOfParameters;

        #region Constructor(s)

        public MyPropertiesOverloadsInfo(string name)
            : base()
        {
            this.name = name;

            this.overloads = new List<MyPropertyInfo>();
        }

        #endregion

        #region Public methods

        public void AddProperty(MyPropertyInfo myPropertyInfo)
        {
            myPropertyInfo.IndexInOverloadsList = overloads.Count;

            overloads.Add(myPropertyInfo);

            if (propertyWithTheSmallestNumberOfParameters == null
             || myPropertyInfo.Parameters.Count < propertyWithTheSmallestNumberOfParameters.Parameters.Count)
            {
                propertyWithTheSmallestNumberOfParameters = myPropertyInfo;
            }
        }

        #endregion

        #region Enumeration

        #region IEnumerable<MyPropertyInfo> Members

        public IEnumerator<MyPropertyInfo> GetEnumerator()
        {
            foreach (MyPropertyInfo MyPropertyInfo in overloads)
            {
                yield return MyPropertyInfo;
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #endregion

        #region Public properties

        public int Count
        {
            get { return overloads.Count; }
        }

        public override MyClassInfo DeclaringType
        {
            get { return overloads.Count == 0 ? null : overloads[0].DeclaringType; }
        }

        public string SummaryWithoutPrefix
        {
            get
            {
                if (propertyWithTheSmallestNumberOfParameters == null)
                {
                    return null;
                }

                return propertyWithTheSmallestNumberOfParameters.Summary;
            }
        }

        #endregion

        #region Public indexers

        public MyPropertyInfo this[int index]
        {
            get { return overloads[index]; }
        }

        #endregion

        #region ISummarisableMember Members

        public string DisplayableName
        {
            get
            {
                if (overloads != null)
                {
                    return overloads[0].DisplayableName;
                }
                else
                {
                    Debug.Assert(false, "Impossible! There must be at least one overload.");

                    return "";
                }
            }
        }

        public override string Summary
        {
            get
            {
                string summaryWithoutPrefix = SummaryWithoutPrefix;

                if (summaryWithoutPrefix == null)
                {
                    return null;
                }

                if (overloads.Count > 1)
                {
                    return "Overloaded. " + summaryWithoutPrefix;
                }
                else
                {
                    return summaryWithoutPrefix;
                }
            }
        }

        #endregion

        #region MetaClass overrides

        public override string GetMetaName()
        {
            return "Property Overloads";
        }

        #endregion
    }
}
