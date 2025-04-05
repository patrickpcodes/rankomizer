using CommunityToolkit.Mvvm.Input;
using Rankomizer.Mobile.Models;

namespace Rankomizer.Mobile.PageModels;

public interface IProjectTaskPageModel
{
	IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
	bool IsBusy { get; }
}