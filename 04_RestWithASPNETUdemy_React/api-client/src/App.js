import React from 'react';
import './global.css';
//import Login from './pages/Login';
//import Routes from './routes';
import Rotas from './routes.js';

export default function App() {
  //let counter = 0;

  //Como se fosse um Array que carrega um valor e uma função [value, changeValueFunction] 
  //Ao invés de mudar o valor na variável, vamos chamar a função e ela sim muda o variável.
  //const [counter, setCounter] = useState(0);

  /*function increment(){
    setCounter(counter + 1);
  }*/
  return(
    //JSX (JavaScript XML)

    //(1)
    //<h1>Vai RESTFul!</h1>

    //(2) - Usando Propriedades - props
    //<Header title="Teste utilizando Cabeçalho com propriedades"/> 

    //(3) - Usando Propriedades - children
    /*<div>
      <Header>
        Counter: {counter}
      </Header>
      <button onClick={increment}>Add</button>
    </div>*/
    //<Login/>]

    <Rotas/>
  );
}
