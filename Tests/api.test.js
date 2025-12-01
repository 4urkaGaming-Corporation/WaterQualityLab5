const request = require('supertest');

const apiUrl = process.env.API_URL || 'http://localhost:5000';

describe('WaterQuality API v1/v2 integration tests', () => {
  test('GET /api/v1/stations повинно повертати список', async () => {
    const res = await request(apiUrl).get('/api/v1/stations');
    expect(res.statusCode).toBe(200);
    expect(Array.isArray(res.body)).toBe(true);
  });

  test('POST /api/v1/stations створює станцію', async () => {
    const station = { name: 'Test Station', location: 'Test City' };
    const res = await request(apiUrl)
      .post('/api/v1/stations')
      .send(station)
      .set('Content-Type', 'application/json');

    expect(res.statusCode).toBe(201);
    expect(res.body.id).toBeDefined();
  });

  test('GET /api/v2/measurements повертає список (v2 DTO)', async () => {
    const res = await request(apiUrl).get('/api/v2/measurements?page=1&pageSize=5');
    expect(res.statusCode).toBe(200);
    expect(Array.isArray(res.body)).toBe(true);
  });
});
