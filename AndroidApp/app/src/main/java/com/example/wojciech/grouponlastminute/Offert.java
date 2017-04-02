package com.example.wojciech.grouponlastminute;

/**
 * Created by Wojciech on 27.03.2017.
 */

public class Offert {
    private String 	id, idClientBusiness, description, 	title, price, quantity, deadlineTime, clientTitle;

    public Offert(String id, String idClientBusiness, String description, String title, String price, String quantity, String deadlineTime) {
        this.id = id;
        this.idClientBusiness = idClientBusiness;
        this.description = description;
        this.title = title;
        this.price = price;
        this.quantity = quantity;
        this.deadlineTime = deadlineTime;
    }

    public Offert(String id, String title, String Quantity, String ClientTitle, String deadlineTime){
        this.id = id;
        this.clientTitle = ClientTitle;
        this.title = title;
        this.quantity = Quantity;
        this.deadlineTime = deadlineTime;
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getIdClientBusiness() {
        return idClientBusiness;
    }

    public void setIdClientBusiness(String idClientBusiness) {
        this.idClientBusiness = idClientBusiness;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public String getPrice() {
        return price;
    }

    public void setPrice(String price) {
        this.price = price;
    }

    public String getQuantity() {
        return quantity;
    }

    public void setQuantity(String quantity) {
        this.quantity = quantity;
    }

    public String getDeadlineTime() {
        return deadlineTime;
    }

    public void setDeadlineTime(String deadlineTime) {
        this.deadlineTime = deadlineTime;
    }
}
