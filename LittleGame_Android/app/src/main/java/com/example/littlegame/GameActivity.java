package com.example.littlegame;

import androidx.appcompat.app.AppCompatActivity;
import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;

import android.Manifest;
import android.app.Activity;
import android.content.Intent;
import android.content.pm.ActivityInfo;
import android.content.pm.PackageManager;
import android.os.Bundle;
import android.util.Log;

public class GameActivity extends Activity {

    private DrawView drawView;
    private String severIp;
    public DrawView getDrawView() { return drawView; }
    public String getSeverIp() { return severIp; }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_LANDSCAPE);
        Bundle bundle = getIntent().getExtras();
        severIp = bundle.getString("ip");
        drawView = new DrawView(this);
        setContentView(drawView);

    }

    public void BackToMainActivity(){
        Intent intent = new Intent();
        intent.setClass(GameActivity.this, MainActivity.class);
        startActivity(intent);
    }
}