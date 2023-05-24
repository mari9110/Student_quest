const mongoose = require('mongoose');
const {Schema} = mongoose;

const ratingSchema = new Schema({
    username: String,
    score: Number,
});

mongoose.model('ratingLines', ratingSchema);
