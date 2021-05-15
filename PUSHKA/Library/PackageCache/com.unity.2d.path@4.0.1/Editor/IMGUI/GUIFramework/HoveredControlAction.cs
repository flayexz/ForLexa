break;
				case MessageType.Unpause:
					EditorApplication.isPaused = false;
					break;
				case MessageType.Build:
					// Not used anymore
					break;
				case MessageType.Refresh:
					Refresh();
					break;
				case MessageType.Version:
					Answer(message, MessageType.Version, PackageVersion());
					break;
				case MessageType.UpdatePackage:
					// Not used anymore
					break;
				case MessageType.ProjectPath:
					Answer(message, MessageType.ProjectPath, Path.GetFullPath(Path.Combine(Application.dataPath, "..")));
					break;
				case MessageType.ExecuteTests:
					TestRunnerApiListener.ExecuteTests(message.Value);
					break;
				case MessageType.RetrieveTestList:
					TestRunnerAp