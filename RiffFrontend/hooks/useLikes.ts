// hooks/useLikes.ts
import { create } from 'zustand';
import { Envelope, Track } from '@/types';

interface LikeStore {
  likedTrackIds: Set<string>;
  isLoading: boolean;
  toggleLike: (trackId: string) => void;
  setLiked: (trackId: string, liked: boolean) => void;
  fetchUserLikesIds: (id: string) => Promise<void>;
}

export const useLike = create<LikeStore>((set, get) => ({
  likedTrackIds: new Set<string>(),
  isLoading: true,

  toggleLike: (trackId) => {
    set((state) => {
      const newSet = new Set(state.likedTrackIds);
      if (newSet.has(trackId)) {
        newSet.delete(trackId);
      } else {
        newSet.add(trackId);
      }
      return { likedTrackIds: newSet };
    });
  },

  setLiked: (trackId, liked) => {
    set((state) => {
      const newSet = new Set(state.likedTrackIds);
      if (liked) newSet.add(trackId);
      else newSet.delete(trackId);
      return { likedTrackIds: newSet };
    });
  },

  fetchUserLikesIds: async (id: string) => {
    try {
      var response = await fetch(`http://localhost:8080/api/track/like/${id}`, {
          credentials: "include",
          cache: "no-store",
      });
      
      const envelope = (await response.json()) as Envelope<Track[]>;

      if (envelope.errors && envelope.errors.length > 0) {
          console.error("API Errors:", envelope.errors);
      }

      const trackIds = envelope.result!.map((t: any) => t.id);
      set({ 
        likedTrackIds: new Set(trackIds || []), 
        isLoading: false 
      });
      
    } catch (err) {
      console.error("Ошибка сети при загрузке лайков", err);
      set({ likedTrackIds: new Set(), isLoading: false });
    }
  },
}));