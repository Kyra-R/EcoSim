const express = require('express');
const mongoose = require('mongoose');
const bodyParser = require('body-parser');
const cors = require('cors');
const crypto = require('crypto');
const SALT = 'secretingredient';

const app = express();
app.use(cors());
app.use(bodyParser.json({ limit: '16mb' }));
app.use(bodyParser.urlencoded({ limit: '16mb', extended: true }));

mongoose.connect(process.env.MONGODB_URI || 'mongodb://localhost:27017/ecosim', {
    useNewUrlParser: true,
    useUnifiedTopology: true
});

// Функция хэширования userId
function hashUserId(userId) {
    return crypto.createHash('sha256').update(SALT + userId).digest('hex');
}

// Схема MongoDB 
const saveSchema = new mongoose.Schema({
    hashedUserId: String,
    saveData: Object,
    timestamp: Date
}, { collection: 'Saves' });

const Save = mongoose.model('Save', saveSchema);

app.get('/ping', (req, res) => {
    res.send('pong');
});

app.post('/upload', async (req, res) => {
  try {
    const rawUserId = req.headers['user-id'] || 'unknown';
    const hashedUserId = hashUserId(rawUserId);
    const saveData = req.body;
	
	console.log('Received userId:', req.headers['user-id']);
	console.log('Hashed userId:', hashUserId(req.headers['user-id']));

    if (!rawUserId || !saveData) {
      return res.status(400).json({ message: 'Missing userId or saveData' });
    }

    const userSaves = await Save.find({ hashedUserId }).sort({ timestamp: 1 });

    if (userSaves.length >= 2) {
      const oldest = userSaves[0];
      await Save.deleteOne({ _id: oldest._id });
    }

    const newSave = new Save({
      hashedUserId,
      saveData,
      timestamp: new Date()
    });

    await newSave.save();

    res.status(200).json({ message: 'Save uploaded successfully.' });

  } catch (err) {
    console.error('Upload error:', err);
    res.status(500).json({ message: 'Server error during upload.' });
  }
});

app.get('/download', async (req, res) => {
  try {
    const rawUserId = req.headers['user-id'];
    const hashedUserId = hashUserId(rawUserId);
	
	console.log('Received userId:', req.headers['user-id']);
	console.log('Hashed userId:', hashUserId(req.headers['user-id']));

    const save = await Save.findOne({ hashedUserId }).sort({ timestamp: -1 });

    if (!save) return res.status(404).send('No save found');

    res.json(save.saveData);
  } catch (err) {
    console.error('Download error:', err);
    res.status(500).send('Server error during download.');
  }
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Сервер запущен на порту ${PORT}`));