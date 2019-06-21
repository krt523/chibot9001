using System;
using System.Collections.Generic;
using System.IO;
using ChiBot.TwitchClient.CommandHandler;
using ChiBot.Domain.Models;
using System.Threading.Tasks;

namespace ChiBot.TwitchClient
{
    public class IrcClient 
    {
        //Connection information
        private string _serverHostname;
        private int _serverPort;

        //network resources
        private StreamReader _inputStream;
        private StreamWriter _outputStream;
        private System.Net.Sockets.TcpClient _tcpClient;

        //twitch channel/login
        private string _nickname, _password;
        private IChannelBroker _channelBroker;
        private List<TwitchChannel> _channels;

        //everything else
        private IEnumerable<ICommandHandler> _commandHandlers;
        private bool exit; //Keeps track of if the client should exit during the control loop.

        public IrcClient(ClientConfiguration configuration, IEnumerable<ICommandHandler> commandHandlers, IChannelBroker channelBroker) {
            _nickname = configuration.Nickname;
            _password = configuration.OauthToken;
            _serverHostname = configuration.ServerHostName;
            _serverPort = configuration.ServerPort;
            _channelBroker = channelBroker;
            _channels = _channelBroker.RequestChannelAssignments("HARDCODED");
            _commandHandlers = commandHandlers;
        }

        public void Start() {
            //Connect and configure streams.
            _tcpClient = new System.Net.Sockets.TcpClient();
            _tcpClient.Connect(_serverHostname, _serverPort);
            _inputStream = new StreamReader(_tcpClient.GetStream());
            _outputStream = new StreamWriter(_tcpClient.GetStream());

            BufferCommand("PASS " + _password);
            BufferCommand("NICK " + _nickname);
            _outputStream.Flush();

            //wait to be connected, join channels.        
            if (_inputStream.ReadLine().Contains("001"))
            {
                foreach(TwitchChannel channel in _channels)
                {
                    BufferCommand("JOIN #" + channel.Name);
                } 
                _outputStream.Flush();
            }

            //start control loop
            ControlLoop();
        }

       //causes us to exit the control loop on next cycle.
        public void Exit() {
            this.exit = true;
        }

        //logic for actually monitoring channel.
        private void ControlLoop()
        {

            while (!exit)
            {
                
                string currentTextLine = _inputStream.ReadLine();

                /* IRC commands come in one of these formats:
                 * :NICK!USER@HOST COMMAND ARGS ... :DATA\r\n
                 * :SERVER COMAND ARGS ... :DATA\r\n
                 */

                //Display received irc message
                Console.WriteLine(currentTextLine);
                //if (currentTextLine[0] != ':') continue;

                //Send pong reply to any ping messages
                if (currentTextLine.StartsWith("PING "))
                {
                    _outputStream.Write(currentTextLine.Replace("PING", "PONG") + "\r\n");
                    _outputStream.Flush();
                }
                else if (currentTextLine[0] != ':')
                {
                    continue;
                }
                else if (BotCommand.isPotentialCommand(currentTextLine))
                {
                    BotCommand command = new BotCommand(currentTextLine);
                    TwitchChannel channel = _channels.Find(c => '#' + c.Name == command.Channel);
                    foreach (ICommandHandler handler in _commandHandlers)
                    {
                        CommandHandlerResult result = handler.ProcessCommand(command, channel).Result;
                        bool breakLoop = false;
                        switch (result.ResultType)
                        {
                            case ResultType.Handled:
                                breakLoop = true;
                                break;
                            case ResultType.HandledWithMessage:
                                SendPrivmsg(result.Channel, result.Message);
                                breakLoop = true;
                                break;
                            default:
                                break;
                        }
                        if (breakLoop)
                            break;
                    }
                }
            }
        }

        private void BufferCommand(string command) {
            _outputStream.Write(command + "\r\n");
        }

        //Sends a private Message. Target must be formatted correctly.
        public void SendPrivmsg(string target, string response) {
            _outputStream.Write("PRIVMSG " +  target + " :" + response + "\r\n");
            _outputStream.Flush();
        }

        private void JoinChannel(string channel) {
            _outputStream.Write("JOIN #" + channel + "\r\n");
        }
    }
}
