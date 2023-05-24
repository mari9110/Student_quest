const express = require("express");
const app = express();
const mongoose = require('mongoose');

require('./model/Ratingline');
const Ratingline = mongoose.model('ratingLines');

const { MongoClient } = require("mongodb");

const uri = process.env.MONGODB_URI;
mongoose.connect(uri, {useNewUrlParser: true, useUnifiedTopology: true});

app.get('/rline', async (req,res) => {
  const client = new MongoClient(uri, { useUnifiedTopology: true });
  
  try {
    const {rusername, rscore} = req.query;
        var ratLine = await Ratingline.findOne({username: rusername, score: rscore});
        if (ratLine == null ){
            var newRline = new Ratingline({
                username: rusername,
                score: rscore
            });
            await newRline.save();
        }
        var result = await Ratingline.find({},{_id:0}).sort({"score":-1}).limit(10);
        console.log(result); //в консоль баш
        res.send(result);
        return;

    
  } catch(err) {
    console.log(err);
  }
});

// start the server listening for requests
app.listen(process.env.PORT || 3000, 
	() => console.log("Server is running..."));
