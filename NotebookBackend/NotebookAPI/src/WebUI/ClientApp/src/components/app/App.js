import './App.css';
import {NoteList} from '../NoteList';
import {Navigation} from '../Navigation';
import {CategoryList} from '../CategoryList';
import {BrowserRouter, Route, Switch} from 'react-router-dom';

function App() {
  return (
    <BrowserRouter>
      <div className="container">
        <Navigation/>

        <Switch>
          <Route path='/' component={NoteList} exact />
          <Route path='/category-list' component={CategoryList}/>
        </Switch>
      </div>
    </BrowserRouter>
  );
}

export default App;
