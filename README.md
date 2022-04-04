# Wellness Journal

*Wellness Journal* is an app developed in Unity designed for Android and iOS mobile platforms. Its functionality is twofold.

First, it is a daily checklist of medications scheduled to be taken by the user. The user defines medications that will be taken at designated times each day, and on certain days of the week. The app measures the user's consistency in taking their medications across weeks and months, assigning the user grades which can be viewed in context of a calendar month.

Second, it is a microjournal for the user to record their mood or changes in symptoms each day. The user can also rate each day from 1-5. The calendar view will also allow the user to view their mood across a calendar month, and to view records of past days to contextualize the mood or symptoms they experience.

## Setup & Running

The app was developed using Unity Editor Version 2020.3.27f1. Having installed this version of the Unity Editor from the [Unity website](https://unity3d.com/get-unity/download), open this project in the editor. You can run the project directly from the editor by clicking the 'play' button at the top of the screen.

Alternatively, you may build this app as an EXE, as an APK for Android devices, or as an iOS app (only possible when running Unity on a Mac computer). Navigate to File -> Build Settings to view a list of platforms, click 'Switch Platform' if the one viewed is not already selected, and then click 'Build' to build the app for that platform. Refer to the [Unity documentation](https://docs.unity3d.com/Manual/BuildSettings.html) for further guidance if needed.

### Unit Testing

Since the app's functionality depends largely on consistent usage, it is difficult to create unit tests for any individual feature.

Nevertheless, we have developed two unit tests for use in the Unity Editor.

## Recent Features

Since we first demonstrated the project, we have implemented the following new features:

- **Custom Dose Icons:** The user can change the icon for a scheduled dose by selecting its shape and color from the medication editor view.
- **Microjournal Tab:** The user can view all microjournal entries in chronological order, and click on an entry to view its associated day record.
