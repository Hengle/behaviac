﻿////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2009, Daniel Kollmann
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, are permitted
// provided that the following conditions are met:
//
// - Redistributions of source code must retain the above copyright notice, this list of conditions
//   and the following disclaimer.
//
// - Redistributions in binary form must reproduce the above copyright notice, this list of
//   conditions and the following disclaimer in the documentation and/or other materials provided
//   with the distribution.
//
// - Neither the name of Daniel Kollmann nor the names of its contributors may be used to endorse
//   or promote products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR
// IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
// WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY
// WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
////////////////////////////////////////////////////////////////////////////////////////////////////

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// The above software in this distribution may have been modified by THL A29 Limited ("Tencent Modifications").
//
// All Tencent Modifications are Copyright (C) 2015 THL A29 Limited.
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Behaviac.Design;
using Behaviac.Design.Attributes;
using PluginBehaviac.Properties;
using Behaviac.Design.Nodes;

namespace PluginBehaviac.Nodes
{
    [NodeDesc("Actions", NodeIcon.Wait)]
    public class Wait : Behaviac.Design.Nodes.Node
	{
		public Wait() : base(Resources.Wait, Resources.WaitDesc)
		{
			_exportName = "Wait";
		}

        public override string ExportClass
        {
            get { return "Wait"; }
        }

        protected VariableDef _time = new VariableDef(1000.0f);
        [DesignerPropertyEnum("Duration", "DurationDesc", "CategoryBasic", DesignerProperty.DisplayMode.Parameter, 1, DesignerProperty.DesignerFlags.NoFlags, DesignerPropertyEnum.AllowStyles.ConstAttributes, "", "", ValueTypes.Float)]
        public VariableDef Time
        {
            get { return _time; }
            set { this._time = value; }
        }

        public override void PostCreate(List<Node.ErrorCheck> result, int version, System.Xml.XmlNode xmlNode)
        {
            if (_time != null && _time.IsConst && _time.Value is int)
                _time.Value = 1.0f * (int)_time.Value;
        }

        protected override void CloneProperties(Node newnode)
        {
            base.CloneProperties(newnode);

            Wait dec = (Wait)newnode;
            if (_time != null)
                dec._time = (VariableDef)_time.Clone();
        }

        private readonly static Brush __defaultBackgroundBrush = new SolidBrush(Color.FromArgb(157, 75, 39));
        protected override Brush DefaultBackgroundBrush
        {
            get { return __defaultBackgroundBrush; }
        }

        public override NodeViewData CreateNodeViewData(NodeViewData parent, Behaviac.Design.Nodes.BehaviorNode rootBehavior)
        {
            NodeViewData nvd = base.CreateNodeViewData(parent, rootBehavior);
            nvd.ChangeShape(NodeShape.Rectangle);

            return nvd;
        }

        public override void CheckForErrors(BehaviorNode rootBehavior, List<ErrorCheck> result)
        {
            Type valueType = (this._time != null) ? this._time.GetValueType() : null;

            if (valueType == null)
            {
                result.Add(new Node.ErrorCheck(this, ErrorCheckLevel.Error, "Time is not set!"));
            }
            else
            {
                if (!Plugin.IsIntergerType(valueType) && !Plugin.IsFloatType(valueType))
                {
                    result.Add(new Node.ErrorCheck(this, ErrorCheckLevel.Error, "Time should be a float number type!"));
                }
            }

            base.CheckForErrors(rootBehavior, result);
        }

        public override bool ResetMembers(bool check, AgentType agentType, bool clear, MethodDef method = null, PropertyDef property = null)
        {
            bool bReset = false;

            if (this._time != null)
            {
                bReset |= this._time.ResetMembers(check, agentType, clear, property);
            }

            bReset |= base.ResetMembers(check, agentType, clear, method, property);

            return bReset;
        }
	}
}
