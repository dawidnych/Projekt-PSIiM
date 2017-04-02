package com.example.wojciech.grouponlastminute;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.widget.TextView;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.StringRequest;
import com.android.volley.toolbox.Volley;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

public class OffertDescription extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_offert_description);
        // Get the Intent that started this activity and extract the string
        Intent intent = getIntent();
        String message = intent.getStringExtra(MainActivity.EXTRA_MESSAGE);

        // Capture the layout's TextView and set the string as its text


        fillUp(message);
    }

    private void fillUp(String message) {
        // Instantiate the RequestQueue.
        RequestQueue queue = Volley.newRequestQueue(this);
        String url = "http://ec2-35-158-35-241.eu-central-1.compute.amazonaws.com/groupon/get_offert_details.php?oid=" + message;

        // Request a string response from the provided URL.
        StringRequest stringRequest = new StringRequest(Request.Method.GET, url,
                new Response.Listener<String>() {
                    @Override
                    public void onResponse(String response) {

                        try {
                            JSONObject obj = new JSONObject(response);
                            JSONArray arr = obj.getJSONArray("offert");

                            String title = arr.getJSONObject(0).getString("title");
                            String description = arr.getJSONObject(0).getString("description");
                            String quantity = arr.getJSONObject(0).getString("quantity");
                            String deadlineTime = arr.getJSONObject(0).getString("deadlineTime");
                            String ClientTitle = arr.getJSONObject(0).getString("ClientTitle");

                            TextView textView_Title = (TextView) findViewById(R.id.offertTitle);
                            textView_Title.setText(title);

                            TextView textView_description = (TextView) findViewById(R.id.description);
                            textView_description.setText(description);

                            TextView textView_quantity = (TextView) findViewById(R.id.quantity);
                            textView_quantity.setText(quantity);

                            TextView textView_deadlineTime = (TextView) findViewById(R.id.data);
                            textView_deadlineTime.setText(deadlineTime);

                            TextView textView_ClientTitle = (TextView) findViewById(R.id.companyName);
                            textView_ClientTitle.setText(ClientTitle);




                        } catch (JSONException e) {
                            e.printStackTrace();
                        }



                    }
                }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                //do smoething ?
            }
        });
// Add the request to the RequestQueue.
        queue.add(stringRequest);

    }
}
