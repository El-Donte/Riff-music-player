import { create } from 'zustand';
import { persist } from 'zustand/middleware';

interface PlayerState {
  activeId?: string;
  ids: string[];
  shuffledIds: string[];
  isShuffled: boolean;
  volume: number;
  isPlaying: boolean;
  repeatMode: 'off' | 'all' | 'one';

  setId: (id: string) => void;
  setIds: (ids: string[]) => void;
  play: () => void;
  pause: () => void;
  toggleShuffle: () => void;
  toggleRepeat: () => void;
  next: () => void;
  previous: () => void;
  setVolume: (volume: number) => void;
  reset: () => void;

  shuffleArray: (array: string[]) => string[];
  getCurrentPlaylist: () => string[];
}

const usePlayer = create<PlayerState>()(
  persist(
    (set, get) => ({
      activeId: undefined,
      ids: [],
      shuffledIds: [],
      volume: 1,
      isPlaying: false,
      isShuffled: false,
      repeatMode: 'off',

      reset: () => set({ ids: [], shuffledIds: [], activeId: undefined }),

      setId: (id: string) => set({ activeId: id }),

      setIds: (ids: string[]) => {
        const { isShuffled, shuffleArray } = get();
        const shuffledIds = isShuffled ? shuffleArray([...ids]) : [...ids];
        set({ ids, shuffledIds });
      },

      play: () => set({ isPlaying: true }),
      pause: () => set({ isPlaying: false }),

      shuffleArray: (array: string[]) => {
        const arr = [...array];
        for (let i = arr.length - 1; i > 0; i--) {
          const j = Math.floor(Math.random() * (i + 1));
          [arr[i], arr[j]] = [arr[j], arr[i]];
        }
        return arr;
      },

      toggleShuffle: () => {
        const state = get();

        if (state.isShuffled) {
          set({ isShuffled: false });
        } 
        else {
          const shuffled = state.shuffleArray([...state.ids]);
          set({
            isShuffled: true,
            shuffledIds: shuffled,
          });
        }
      },

      toggleRepeat: () =>
        set((state) => ({
          repeatMode:
            state.repeatMode === 'off'
              ? 'all'
              : state.repeatMode === 'all'
              ? 'one'
              : 'off',
        })),

      getCurrentPlaylist: () => {
        const state = get();
        return state.isShuffled ? state.shuffledIds : state.ids;
      },

      next: () => {
        const state = get();
        if (!state.activeId) return;

        const playlist = state.getCurrentPlaylist();
        const currentIndex = playlist.indexOf(state.activeId);

        if (currentIndex === -1) return;

        let nextIndex = currentIndex + 1;

        if (nextIndex >= playlist.length) {
          if (state.repeatMode === 'off') {
            return;
          }
          if (state.repeatMode === 'all') {
            nextIndex = 0;
          }
        }

        set({ activeId: playlist[nextIndex] });
      },

      previous: () => {
        const state = get();
        if (!state.activeId) return;

        const playlist = state.getCurrentPlaylist();
        const currentIndex = playlist.indexOf(state.activeId);

        if (currentIndex <= 0) {
          if (state.repeatMode === 'off')
          { 
            return;
          }
          if (state.repeatMode === 'all') {
            const lastIndex = playlist.length - 1;
            set({ activeId: playlist[lastIndex] });
            return;
          }
        }

        set({ activeId: playlist[currentIndex - 1] });
      },

      setVolume: (volume: number) => set({ volume }),
    }),
    {
      name: 'player-storage',
      partialize: (state) => ({
        volume: state.volume,
        repeatMode: state.repeatMode,
        isShuffled: state.isShuffled,
      }),
    }
  )
);

export default usePlayer;