package com.example.wojciech.grouponlastminute;

import android.content.Context;
import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.support.v7.widget.DefaultItemAnimator;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.view.View;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import com.android.volley.RequestQueue;
import com.android.volley.toolbox.StringRequest;
import com.android.volley.toolbox.Volley;
import com.android.volley.*;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.Date;
import java.util.List;

public class MainActivity extends AppCompatActivity {

    private List<Offert> movieList = new ArrayList<>();
    private RecyclerView recyclerView;
    private OffertAdapter mAdapter;

    public static final String EXTRA_MESSAGE = "com.example.myfirstapp.MESSAGE";


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        Context context = this.getApplicationContext();
        recyclerView = (RecyclerView) findViewById(R.id.recycler_view);

        mAdapter = new OffertAdapter(movieList);
        RecyclerView.LayoutManager mLayoutManager = new LinearLayoutManager(getApplicationContext());
        recyclerView.setLayoutManager(mLayoutManager);
        recyclerView.setItemAnimator(new DefaultItemAnimator());
        recyclerView.addItemDecoration(new DividerItemDecoration(this, LinearLayoutManager.VERTICAL));
        recyclerView.setAdapter(mAdapter);

        recyclerView.addOnItemTouchListener(
                new RecyclerItemClickListener(context, recyclerView ,new RecyclerItemClickListener.OnItemClickListener() {
                    @Override public void onItemClick(View view, int position) {
                        Offert movie = movieList.get(position);
                        Intent intent = new Intent(MainActivity.this, OffertDescription.class);
                        intent.putExtra(EXTRA_MESSAGE, movie.getId().toString());
                        startActivity(intent);
                    }

                    @Override public void onLongItemClick(View view, int position) {
                        // do whatever
                    }
                })
        );
        prepareOffertData();
    }
    void prepareOffertData(){

        // Instantiate the RequestQueue.
        RequestQueue queue = Volley.newRequestQueue(this);
        String url ="http://ec2-35-158-35-241.eu-central-1.compute.amazonaws.com/groupon/get_offerts_list.php";

        // Request a string response from the provided URL.
        StringRequest stringRequest = new StringRequest(Request.Method.GET, url,
                new Response.Listener<String>() {
                    @Override
                    public void onResponse(String response) {

                        try {
                            JSONObject obj = new JSONObject(response);
                            JSONArray arr = obj.getJSONArray("offert");
                            for (int i = 0; i < arr.length(); i++)
                            {

                                String offert_id = arr.getJSONObject(i).getString("id");
                                String title = arr.getJSONObject(i).getString("title");
                                String ClientTitle = arr.getJSONObject(i).getString("ClientTitle");
                                String quantity = arr.getJSONObject(i).getString("quantity");
                                String data = arr.getJSONObject(i).getString("deadlineTime");

                                Offert offert = new Offert(offert_id, title, quantity, ClientTitle, data);
                                movieList.add(offert);
                            }
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
