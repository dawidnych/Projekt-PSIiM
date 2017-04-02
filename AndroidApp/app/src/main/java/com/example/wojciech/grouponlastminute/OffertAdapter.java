package com.example.wojciech.grouponlastminute;

/**
 * Created by Wojciech on 27.03.2017.
 */

import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import java.util.List;

public class OffertAdapter extends RecyclerView.Adapter<OffertAdapter.MyViewHolder> {

    private List<Offert> moviesList;

    public class MyViewHolder extends RecyclerView.ViewHolder {
        public TextView mTitle, mDate, mQuantity;

        public MyViewHolder(View view) {
            super(view);
            mTitle = (TextView) view.findViewById(R.id.title);
            mDate = (TextView) view.findViewById(R.id.date);
            mQuantity = (TextView) view.findViewById(R.id.quantity);
        }
    }


    public OffertAdapter(List<Offert> moviesList) {
        this.moviesList = moviesList;
    }

    @Override
    public MyViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View itemView = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.offert_list_row, parent, false);

        return new MyViewHolder(itemView);
    }

    @Override
    public void onBindViewHolder(MyViewHolder holder, int position) {
        Offert movie = moviesList.get(position);
        holder.mTitle.setText(movie.getTitle());
        holder.mDate.setText(movie.getDeadlineTime());
        holder.mQuantity.setText(movie.getQuantity());
    }

    @Override
    public int getItemCount() {
        return moviesList.size();
    }
}