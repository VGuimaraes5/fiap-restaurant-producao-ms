import http from 'k6/http';
import {sleep, check} from 'k6';

export const options = {
  stages: [
    { duration: '1m', target: 2000 },
    { duration: '1m', target: 0 },
  ],
};

export default () => {
  const responseGetAll = http.get('http://localhost:5003/Producao');
  check(responseGetAll, {
    'Get Categoria ok': (r) => r.status === 200,
  });
  sleep(1);
};
