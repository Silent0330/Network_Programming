package com.example.littlegame;

import android.util.Log;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.net.Socket;
import java.util.Vector;

public class ClientSocketManager {

    private Socket clientSocket;
    private BufferedReader in;
    private BufferedWriter out;
    private String severIp;
    private int port;
    public Thread recvThread, sendThread;
    public Vector<String> messagesToSend;
    public Vector<String> recivedMessages;

    private boolean connecting;
    public boolean getConnecting() { return connecting; }

    private int playerId;
    public int getPlayerId() { return playerId; }
    private int playerNum;
    public int getPlayerNum() { return playerNum; }

    private boolean connected;
    public boolean getConnected() { return connected; }
    private boolean gameStart;
    public boolean getGameStart() { return gameStart; }


    public ClientSocketManager()
    {
        connected = false;
        gameStart = false;
        connecting = false;
        clientSocket = null;
        messagesToSend = new Vector<String>();
        recivedMessages = new Vector<String>();
        playerId = playerNum = 0;
    }

    public void StartConnect(String ip, int port) {
        this.severIp = ip;
        this.port = port;
        if(!connected && !connecting) {
            recvThread = new Thread(connection);
            recvThread.start();
        }
    }

    private Runnable connection = new Runnable() {
        @Override
        public void run() {
            try {
                connecting = true;
                clientSocket = new Socket(severIp, port);
                connected = true;
                if(connected) {
                    in = new BufferedReader(new InputStreamReader(clientSocket.getInputStream()));
                    out = new BufferedWriter(new OutputStreamWriter(clientSocket.getOutputStream(), "BIG5"));
                    sendThread = new Thread(snedMessages);
                    sendThread.start();
                }
                connecting = false;
                recivedMessages.clear();
                while (connected){
                    String message = in.readLine();
                    if(message!=null) {
                        Log.d("Debug", "recv message = " + message);
                        String[] messages = message.split(";");
                        for (int i = 0; i < messages.length; i++)
                        {
                            String[] messageArgs = messages[i].split(",");
                            if (messageArgs[0].equals("Full"))
                            {
                                connected = false;
                                Disconnet();
                            }
                            else if (messageArgs[0].equals("Id"))
                            {
                                playerId = Integer.parseInt(messageArgs[1]);
                            }
                            else if (messageArgs[0].equals("PlayerNum"))
                            {
                                playerNum = Integer.parseInt(messageArgs[1]);
                            }
                            else if (messageArgs[0].equals("Start"))
                            {
                                gameStart = true;
                            }
                            else if (messageArgs[0].equals("Move"))
                            {
                                recivedMessages.add(messages[i]);
                            }
                            else if (messageArgs[0].equals("Face"))
                            {
                                recivedMessages.add(messages[i]);
                            }
                            else if (messageArgs[0].equals("Attack"))
                            {
                                recivedMessages.add(messages[i]);
                            }
                            else if (messageArgs[0].equals("Reload"))
                            {
                                recivedMessages.add(messages[i]);
                            }
                            else if (messageArgs[0].equals("ReloadDone"))
                            {
                                recivedMessages.add(messages[i]);
                            }
                            else if (messageArgs[0].equals("Hitted"))
                            {
                                recivedMessages.add(messages[i]);
                            }
                            else if (messageArgs[0].equals("GameOver"))
                            {
                                gameStart = false;
                            }
                            Thread.sleep(5);
                        }
                    }
                }
            }
            catch (Exception e){
                e.printStackTrace();
                Disconnet();
            }
        }
    };

    private Runnable snedMessages = new Runnable() {
        @Override
        public void run() {
            try {
                messagesToSend.clear();
                String message = "";
                while (connected){
                    if(messagesToSend.size() > 0){
                        message = messagesToSend.remove(0);
                        out.write(message + "\n");
                        out.flush();
                    }
                }
            }
            catch (Exception e){
                e.printStackTrace();
                Disconnet();
            }
        }
    };

    public void AddMessageToSend(String message) {
        if(message.charAt(message.length()-1) != ';') {
            message += ";";
        }
        messagesToSend.add(message);
    }

    public void Disconnet() {
        Log.d("Debug", "disconnect");
        connected = false;
        connecting = false;
        gameStart = false;
        try
        {
            if (clientSocket != null)
            {
                clientSocket.close();
                clientSocket = null;
            }
            if (recvThread != null)
            {
                recvThread.interrupt();
                recvThread = null;
            }
            if (sendThread != null)
            {
                sendThread.interrupt();
                sendThread = null;
            }
        }
        catch (Exception e)
        {
            e.printStackTrace();
        }
    }
}
