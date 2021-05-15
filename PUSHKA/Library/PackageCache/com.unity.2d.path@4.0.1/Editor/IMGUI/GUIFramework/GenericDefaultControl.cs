/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Microsoft.Unity.VisualStudio.Editor.Messaging;
using Microsoft.Unity.VisualStudio.Editor.Testing;
using UnityEditor;
using UnityEngine;
using MessageType = Microsoft.Unity.VisualStudio.Editor.Messaging.MessageType;

namespace Microsoft.Unity.VisualStudio.Editor
{
	[InitializeOnLoad]
	internal class VisualStudioIntegration
	{
		class Client
		{
			public IPEndPoint EndPoint { get; set; }
			public DateTime LastMessage { get; set; }
		}

		private static Messager _messager;

		private static readonly Queue<Message> _incoming = new Queue<Message>();
		private static readonly Dictionary<IPEndPoint, Client> _clients = new Dictionary<IPEndPoint, Client>();
		private static readonly object _incomingLock = new object();
		private static readonly object _clientsLock = new object();

		static VisualStudioIntegration()
		{
			if (!VisualStudioEditor.IsEnabled)
				return;

			RunOnceOnUpdate(() =>
			{
				// Despite using ReuseAddress|!ExclusiveAddressUse, we can fail here:
				// - if another application is using this port with exclusive access
				// - or if the firewall is not properly configured
				var messagingPor