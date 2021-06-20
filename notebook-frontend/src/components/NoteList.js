import React,{Component} from 'react';
import {get,remove} from '../helpers/apiHelpers';
import {Table} from 'react-bootstrap';
import {Button} from 'react-bootstrap';
import { NoteFormModal } from './NoteFormModal';

export class NoteList extends Component{
    constructor(props){
        super(props);
        this.state={
            notes: [],
            noteFormModalShow: false,
            noteId: '',
            filter: '',
            order: '',
            refresh: false
        };
    }

    async componentDidMount(){
        await this.handleGetNotes();
    }
    
    async handleGetNotes(){
        let notesList = await get('notes');
        this.setState({notes: notesList});
    }

    async handleDeleteNote(id){
        await remove('notes/'+id);
        this.setState({refresh: true});
    }

    async componentDidUpdate(prevProps, prevState){
        if (!this.state.noteFormModalShow){
            localStorage.removeItem('noteId');
        }

        if(this.state.refresh !== prevState.refresh){
            await this.handleGetNotes();
            
            if(this.state.order){
                this.sortNotes();
            }
            this.setState({
                refresh: false
              })
        }
    }

    sortNotes = () => {
        var sortedList = this.state.notes;
        if(this.state.order === 'asc'){
            sortedList = this.state.notes.sort((a, b) => (b.content.toLowerCase().localeCompare(a.content.toLowerCase())));
            this.setState({order: 'desc'});
        }
        else {
            sortedList = this.state.notes.sort((a, b) => (a.content.toLowerCase().localeCompare(b.content.toLowerCase())));
            this.setState({order: 'asc'});
        }
        this.setState({notes: sortedList})
    }

    render(){
        let noteFormModalClose=()=>{
            this.setState({noteFormModalShow: false});
            this.setState({refresh: true});
        }

        return(
            <div className="mt-4">
                <div id="note-nav">
                    <Button className="create-button" variant='success' onClick={()=>{this.setState({noteFormModalShow: true})}}>
                        Create note
                    </Button>

                    <input type="text" id="search-input" onChange={e => this.setState({filter: e.target.value.toLowerCase()})}
                        placeholder="Search for notes..." />
                </div>
                <div id="sort-order">
                    <Button id="sort-button" variant='secondary' onClick={this.sortNotes}>
                        {this.state.order === 'asc' ? 'Sort descending' : 'Sort ascending'}
                    </Button>
                </div>

                <Table className="mt-4" striped bordered hover size="sm">
                    <thead>
                        <tr>
                            <th>#</th>
                            <th>Content</th>
                            <th className="categories-column">Categories</th>
                            <th className="options-column">Options</th>
                        </tr>
                    </thead>
                    <tbody>
                        {this.state.notes.filter(filteredNotes => {
                            return(
                                    filteredNotes.content.toLowerCase().includes(this.state.filter)
                                );
                            }).map((note, index) => {
                                return(
                                    <tr key={note.id}>
                                        <td>{index+1}</td>
                                        <td>{note.content}</td>
                                        <td>
                                            <ul>
                                                {note.categories.map(category => {
                                                    return(<li key={category.id}>{category.name}</li>);
                                                })}
                                            </ul>
                                        </td>
                                        <td>
                                        <Button className="update-button" variant='warning' onClick={()=>{
                                            localStorage.setItem('noteId', note.id);
                                            this.setState({noteId: note.id});
                                            this.setState({noteFormModalShow: true})
                                            }}>
                                            Update
                                        </Button>
                                        <Button className="delete-button" variant='danger' onClick={()=>{
                                            this.handleDeleteNote(note.id)
                                            }}>
                                            Delete
                                        </Button>
                                        </td>
                                    </tr>
                                )
                        })}
                    </tbody>
                </Table>

                <NoteFormModal show={this.state.noteFormModalShow} noteid={this.state.noteId} onHide={noteFormModalClose} />
            </div>
        )
    }
}