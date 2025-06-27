# POE_Part3_Prog6221_chatbotapplication
#Molebogeng's Cybersecurity Chatbot Program

Welcome to Molebogeng's Cybersecurity Chatbot Program, a C# WPF application designed to simulate real-life scenarios involving cybersecurity. 
The chatbot provides guidance on topics such as password safety, phishing, and recognizing suspicious links. This applicatins also allowers 
users to set/add tasks and the application will ask whether it should remind the user or not, the user should specify when they would like the application to remind them.
I used C# WPF application to develop this application, using the xaml window to display the logo and the voice greeting enhances user interaction in the program. 

##Introduction
The Logo is displayed at the start of the application and allowing users to indulge the wonderful sound of our voice greeting into the application, 
then users are allowed to log in and log out.The goal of this chatbot is to raise awareness and educate users about online security measures. 
It is tailored to answer questions, provide practical tips, and create a secure browsing culture for its users. This application also has a mini Quiz game 
that will test the users knowledge on cybersecurity and staying safe online.

##Features
Displaying the logo image with help from the image tag in the xaml design window
Automatically identifies and accesses the audio file without the need for hardcoding paths.Plays audio files synchronously using the PlaySync() method.
User Interaction:Welcomes the user and personalizes interactions.
Allows users to input questions and receive relevant responses.
Cybersecurity Topics:Password safety tips,Guidance on safe browsing,awareness about phishing tactics and how to avoid them.
Customizable Ignored Words:Filters out unnecessary words (like "what," "is," "your") to focus on keywords in user questions.
Error Handling:Handles exceptions gracefully to ensure the chatbot runs smoothly.

##How It Works
Initialization:
The program initializes by welcoming the user and asking for their name.

Question Handling:
Users type in questions related to cybersecurity.
The chatbot filters out ignored words to identify the essential keywords in the question.

Response Generation:
Matches the keywords against stored replies using LINQ.
If a match is found, the program responds with relevant information.
Otherwise, it prompts the user to search for something security-related.

Exit Condition:Users can exit the program by typing exit.

##System Requirements
.NET Framework installed
IDE such as Visual Studio

##Usage
Run the program and follow the prompts.
Enter your name for a personalized experience.
Type in your cybersecurity-related questions, such as:
"How can I create a strong password?"
"What is phishing?"
"Tips for safe browsing?"

Type exit to quit the application.

##License
This project is licensed under the MIT License
